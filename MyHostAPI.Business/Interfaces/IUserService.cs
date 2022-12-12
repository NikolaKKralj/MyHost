using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Domain;
using MyHostAPI.Models;
using MyHostAPI.Models.Premise;

namespace MyHostAPI.Business.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(RegisterModel registerModel);
        Task<LoginResponseModel> Login(LoginModel loginModel);
        Task<UserResponseModel> UpdateUser(UserUpdateModel updateUserModel, UserContext userContext);
        Task ChangePassword(ChangePasswordModel changePasswordModel, UserContext userContext);
        Task SendMailResetPassword(string email);
        Task ResetPassword(ResetPasswordModel forgotPasswordModel);
        Task<UserResponseModel> GetUser(string id, UserContext userContext);
        Task<bool> IsEmailConfirmed(string email);
        Task<PaginatedList<UserResponseModel>> GetUsersByRole(Role role, Pagination pagination, UserContext userContext);
        Task<string> AddFavoritePremise(string premiseId, UserContext userContext);
        Task<string> RemoveFavoritePremise(string premiseId, UserContext userContext);
        Task<PaginatedList<PremiseResponseModel>> GetFavoritePremises(Pagination pagination, UserContext userContex);
        Task ConfirmEmail(string encryptedEmail);
        Task ConfirmPassword(string password, UserContext userContext);
    }
}
