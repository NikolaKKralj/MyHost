using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Models;

namespace MyHostAPI.Authorization.Interfaces
{
    public interface IAuthorizationHandler<T> where T : class
    {
        /// <summary>
        /// Authorize user to perform operation over resource
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="resource"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        Task Authorize(UserContext userContext, T resource, Operation operation);
    }
}

