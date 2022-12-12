using Microsoft.AspNetCore.Mvc;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Attributes;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Domain;
using MyHostAPI.Models;

namespace MyHostAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            await _userService.CreateUser(registerModel);
            return Ok();
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var loginResponseModel = await _userService.Login(loginModel);
            return Ok(loginResponseModel);
        }

        [Authorize(Role.Admin, Role.Customer, Role.Manager)]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UserUpdateModel userModel)
        {
            var user = await _userService.UpdateUser(userModel, UserContext);
            return Ok(user);
        }

        [Authorize(Role.Admin, Role.Manager, Role.Customer)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            var user = await _userService.GetUser(id, UserContext);
            return Ok(user);
        }

        [AllowAnonymous]
        [Route("password/reset")]
        [HttpPut]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel forgotPasswordModel)
        {
            await _userService.ResetPassword(forgotPasswordModel);
            return Ok();
        }

        [Authorize(Role.Customer, Role.Manager)]
        [Route("password/change")]
        [HttpPut]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            await _userService.ChangePassword(changePasswordModel, UserContext);
            return Ok();
        }

        [Authorize(Role.Admin,Role.Customer,Role.Manager)]
        [Route("password/confirm")]
        [HttpGet]
        public async Task<IActionResult> ConfirmPassword(string password)
        {
            await _userService.ConfirmPassword(password, UserContext);
            return Ok();
        }

        [AllowAnonymous]
        [Route("password/request-reset/{email}")]
        [HttpGet]
        public async Task<IActionResult> SendMailResetPassword(string email)
        {
            await _userService.SendMailResetPassword(email);
            return Ok();
        }

        [AllowAnonymous]
        [Route("email/is-confirmed")]
        [HttpGet]
        public async Task<IActionResult> IsEmailConfirmed(string encryptedEmail)
        {
            return Ok(await _userService.IsEmailConfirmed(encryptedEmail));
        }

        [AllowAnonymous]
        [Route("email/confirm")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string encryptedEmail)
        {
            await _userService.ConfirmEmail(encryptedEmail);
            return Ok();
        }

        [Authorize(Role.Admin)]
        [Route("roles/{role}")]
        [HttpGet]
        public async Task<IActionResult> GetUsersByRole(Role role, [FromQuery] Pagination pagination)
        {
            var users = await _userService.GetUsersByRole(role, pagination, UserContext);
            SetPaginationData(users.Metadata);
            return Ok(users);
        }

        [Authorize(Role.Customer)]
        [HttpGet]
        [Route("favorite-premises")]
        public async Task<IActionResult> GetFavoritePremises([FromQuery] Pagination pagination)
        {
            var premises = await _userService.GetFavoritePremises(pagination, UserContext);
            SetPaginationData(premises.Metadata);
            return Ok(premises);
        }

        [Authorize(Role.Customer)]
        [Route("favorite-premises/{premiseId}")]
        [HttpPut]
        public async Task<IActionResult> AddFavoritePremise(string premiseId)
        {
            var premise = await _userService.AddFavoritePremise(premiseId, UserContext);
            return Ok(premise);
        }

        [Authorize(Role.Customer)]
        [Route("favorite-premises/{premiseId}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveFavoritePremise(string premiseId)
        {
            var premise = await _userService.RemoveFavoritePremise(premiseId, UserContext);
            return Ok(premise);
        }
    }
}
