using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Configurations;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Exceptions;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Data.Specifications;
using MyHostAPI.Domain.Premise;
using MyHostAPI.Domain;
using MyHostAPI.Domain.Reporting;
using MyHostAPI.Models;
using MyHostAPI.Models.Premise;
using static MyHostAPI.Data.Specifications.PremiseSpecification;
using MyHostAPI.Reporting.Interfaces;

namespace MyHostAPI.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IMediaService _mediaService;
        private readonly IPremiseRepository _premiseRepository;
        private readonly IAuthorizationHandler<User> _authorizationHandler;
        private readonly JwtSection _jwtSection;
        private readonly EncryptionSection _encryptionSection;
        private readonly IEmailService _emailService;
        private readonly SendGridEmailSettingsSection _sendGridEmailSettingsSection;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<UserService> logger,
            IAuthorizationHandler<User> authorizationHandler,
            IMediaService mediaService,
            IEmailService emailService,
            IOptions<SendGridEmailSettingsSection> storageOptions,
            IPremiseRepository premiseRepository)
        {
            _jwtSection = configuration.GetSection(JwtSection.Name).Get<JwtSection>();
            _encryptionSection = configuration.GetSection(EncryptionSection.Name).Get<EncryptionSection>();
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
            _mediaService = mediaService;
            _premiseRepository = premiseRepository;
            _authorizationHandler = authorizationHandler;
            _emailService = emailService;
            _sendGridEmailSettingsSection = storageOptions.Value;
        }

        public async Task CreateUser(RegisterModel registerModel)
        {
            var existingUser = await _userRepository.FindOneByAsync(new UserByUsername(registerModel.Email));
            if (existingUser != null)
            {
                _logger.LogError($"User with email {registerModel.Email} already exists.");
                throw new UniqueException($"User with email {registerModel.Email} already exists.");
            }

            registerModel.Password = PasswordHandler.HashPassword(registerModel.Password);

            _logger.LogInformation("Password is hashed.");

            var user = _mapper.Map<User>(registerModel);
            user.Identity.Role = Role.Customer;

            await _userRepository.CreateAsync(user);

            var encryptedEmail = EncryptionHandler.EncryptString(registerModel.Email, _encryptionSection);

            _logger.LogInformation("Email is encrypted.");

            await _emailService.SendEmail(new ConfirmationEmail()
            {
                ActivationLink = _sendGridEmailSettingsSection.ConfirmationLink,
                ToEmail = registerModel.Email
            });

            _logger.LogInformation("Email added to queue.");
        }

        public async Task<UserResponseModel> UpdateUser(UserUpdateModel updateUserModel, UserContext userContext)
        {
            var existingUser = await _userRepository.GetAsync(updateUserModel.Id);

            await _authorizationHandler.Authorize(userContext, existingUser, Operation.UpdateOperation);

            var user = _mapper.Map(updateUserModel, existingUser);

            if (updateUserModel.Image != null)
            {
                var existingImageUrl = existingUser.ProfileImage;

                if (existingImageUrl != null)
                {
                    await _mediaService.DeleteImage(existingImageUrl);
                }

                var imageUrl = await _mediaService.UploadImage(updateUserModel.Image);
                user.ProfileImage = imageUrl;
            }
            else
            {
                user.ProfileImage = updateUserModel.ProfileImage;
            }

            user.Identity = existingUser.Identity;

            await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserResponseModel>(user);
        }

        public async Task<LoginResponseModel> Login(LoginModel loginModel)
        {
            var user = await _userRepository.FindOneByAsync(new UserByUsername(loginModel.Email));

            var hashedPassword = PasswordHandler.HashPassword(loginModel.Password);

            _logger.LogInformation("Password is hashed.");

            if (user == null || user.Identity.Password != hashedPassword)
            {
                _logger.LogError("User doesn't exist.");
                throw new InvalidLoginException($"Invalid username or password.");
            }

            if (!user.Identity.EmailConfirmed)
            {
                _logger.LogError("Email not confirmed.");
                throw new InvalidLoginException($"Email not confirmed.");
            }

            var userResponse = _mapper.Map<UserResponseModel>(user);

            _logger.LogInformation("Creating token.");

            return new LoginResponseModel()
            {
                Token = TokenHandler.CreateToken(loginModel.Email, _jwtSection, user.Id, user.Identity.Role),
                User = userResponse
            };
        }

        public async Task ChangePassword(ChangePasswordModel changePasswordModel, UserContext userContext)
        {
            var user = await _userRepository.FindOneByAsync(new UserByUsername(userContext.Email));

            await _authorizationHandler.Authorize(userContext, user, Operation.UpdateOperation);

            if (PasswordHandler.HashPassword(changePasswordModel.NewPassword) == user.Identity.Password)
            {
                throw new SamePasswordException("New password can not be same as old password.");
            }

            user.Identity.Password = PasswordHandler.HashPassword(changePasswordModel.NewPassword);

            _logger.LogInformation("Password is hashed.");

            await _userRepository.UpdateAsync(user);
        }

        public async Task SendMailResetPassword(string email)
        {
            var user = await _userRepository.FindOneByAsync(new UserByUsername(email));

            if (user == null)
            {
                throw new RecordNotFoundException($"User with email {email}");
            }

            var encryptedEmail = EncryptionHandler.EncryptString(email, _encryptionSection);

            _logger.LogInformation("Sending email for password reset.");

            await _emailService.SendEmail<ResetPasswordEmail>(new ResetPasswordEmail()
            {
                ActivationLink = _sendGridEmailSettingsSection.ResetPasswordLink + encryptedEmail,
                ToEmail = email
            });
        }

        public async Task ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var email = EncryptionHandler.DecryptString(resetPasswordModel.EncryptedEmail, _encryptionSection);

            var user = await _userRepository.FindOneByAsync(new UserByUsername(email));

            if (PasswordHandler.HashPassword(resetPasswordModel.Password) == user.Identity.Password)
            {
                throw new SamePasswordException("New password can not be same as old password.");
            }

            user.Identity.Password = PasswordHandler.HashPassword(resetPasswordModel.Password);

            _logger.LogInformation("Password is hashed.");

            await _userRepository.UpdateAsync(user);
        }

        public async Task<PaginatedList<UserResponseModel>> GetUsersByRole(Role role, Pagination pagination, UserContext userContext)
        {
            var users = await _userRepository.FindManyByAsync(new UsersByRole(role), pagination);

            users.ForEach(async x => await _authorizationHandler.Authorize(userContext, x, Operation.ReadOperation));

            return _mapper.Map<PaginatedList<UserResponseModel>>(users);
        }

        public async Task<UserResponseModel> GetUser(string id, UserContext userContext)
        {
            var user = await _userRepository.FindOneByAsync(new UserById(id));

            await _authorizationHandler.Authorize(userContext, user, Operation.ReadOperation);

            var userModel = _mapper.Map<UserResponseModel>(user);

            return userModel;
        }

        public async Task<bool> IsEmailConfirmed(string encryptedEmail)
        {
            var email = EncryptionHandler.DecryptString(encryptedEmail, _encryptionSection);

            _logger.LogInformation("Decrypting string is done.");

            var user = await _userRepository.FindOneByAsync(new UserByEmail(email));

            return user.Identity.EmailConfirmed;
        }

        public async Task<string> AddFavoritePremise(string premiseId, UserContext userContext)
        {
            var user = await _userRepository.FindOneByAsync(new UserById(userContext.UserId));

            await _authorizationHandler.Authorize(userContext, user, Operation.UpdateOperation);

            var premise = await _premiseRepository.FindOneByAsync(new PremiseById(premiseId)) ?? throw new RecordNotFoundException($"Premise with Id: {premiseId}, does not exist.");

            user.FavoritePremises.Add(premise.Id);

            _logger.LogInformation("Premise added into favourite list.");

            await _userRepository.UpdateAsync(user);

            return premise.Id;
        }

        public async Task<string> RemoveFavoritePremise(string premiseId, UserContext userContext)
        {
            var user = await _userRepository.FindOneByAsync(new UserById(userContext.UserId));

            await _authorizationHandler.Authorize(userContext, user, Operation.UpdateOperation);

            var premise = user.FavoritePremises.FirstOrDefault(x => x == premiseId) ?? throw new RecordNotFoundException($"Premise with Id: {premiseId} is not in favorite Premises.");

            user.FavoritePremises.Remove(premise);

            _logger.LogInformation("Premise removed from favourite list.");

            await _userRepository.UpdateAsync(user);

            return premise;
        }

        public async Task<PaginatedList<PremiseResponseModel>> GetFavoritePremises(Pagination pagination, UserContext userContext)
        {
            var user = await _userRepository.FindOneByAsync(new UserById(userContext.UserId));

            await _authorizationHandler.Authorize(userContext, user, Operation.ReadOperation);

            var premises = await _premiseRepository.FindManyByAsync(new PremisesByFavoritePremiseList(user.FavoritePremises), pagination);

            return _mapper.Map<PaginatedList<PremiseResponseModel>>(premises);
        }

        public async Task ConfirmEmail(string encryptedEmail)
        {
            var email = EncryptionHandler.DecryptString(encryptedEmail, _encryptionSection);

            _logger.LogInformation("Decrypting string is done.");

            var user = await _userRepository.FindOneByAsync(new UserByEmail(email));

            user.Identity.EmailConfirmed = true;

            await _userRepository.UpdateAsync(user);
        }

        public async Task ConfirmPassword(string password, UserContext userContext)
        {
            var user = await _userRepository.FindOneByAsync(new UserById(userContext.UserId));

            var hashedPassword = PasswordHandler.HashPassword(password);

            _logger.LogInformation("Password is hashed.");

            if (user.Identity.Password != hashedPassword)
            {
                throw new UnauthorizedException($"Wrong password for user {user.Identity.Email}.");
            }
        }
    }
}
