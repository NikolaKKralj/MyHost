using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Models;
using MyHostAPI.Domain;

namespace MyHostAPI.Authorization.Validations
{
    public abstract class BaseValidations<T> : IValidations<T> where T : class
    {
        protected abstract Dictionary<Operation, Func<UserContext, T, bool>[]> AdminValidations { get; }
        protected abstract Dictionary<Operation, Func<UserContext, T, bool>[]> ManagerValidations { get; }
        protected abstract Dictionary<Operation, Func<UserContext, T, bool>[]> CustomerValidations { get; }
        public abstract Dictionary<Role, Dictionary<Operation, Func<UserContext, T, bool>[]>> Rules { get; }

        protected static Func<UserContext, T, bool>[] NoSpecificRules() => Array.Empty<Func<UserContext, T, bool>>();
        protected static Func<UserContext, T, bool>[] SpecificRules(params Func<UserContext, T, bool>[] funcs) => funcs.ToArray();
    }
}

