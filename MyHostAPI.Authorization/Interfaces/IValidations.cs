using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Models;
using MyHostAPI.Domain;

namespace MyHostAPI.Authorization.Interfaces
{
    public interface IValidations<T> where T : class
    {
        /// <summary>
        /// Set of specific Rules, based on Role and Operation
        /// </summary>
        abstract Dictionary<Role, Dictionary<Operation, Func<UserContext, T, bool>[]>> Rules { get; }
    }
}

