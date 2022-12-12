using Microsoft.AspNetCore.Mvc;
using MyHostAPI.Common.Exceptions;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Domain;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MyHostAPI.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        public UserContext UserContext
        {
            get => CurrentUserContext();
        }

        protected void SetPaginationData(PaginatedListMetadata metadata)
        {
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
        }

        private UserContext CurrentUserContext()
        {
            try
            {
                var userId = HttpContext.User.Claims.First(x => x.Type == "id").Value;
                var userEmail = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
                var userRole = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
                Enum.TryParse<Role>(userRole, out Role role);

                var userContext = new UserContext(userId, userEmail, role);
                return userContext;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedException($"User context is required but it's null! {ex.Message}");
            }

        }
    }
}
