using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Models;
using MyHostAPI.Domain;

namespace MyHostAPI.Authorization.Validations
{
    public class UserValidations : BaseValidations<User>, IValidations<User>
    {
        public override Dictionary<Role, Dictionary<Operation, Func<UserContext, User, bool>[]>> Rules { get; }

        protected override Dictionary<Operation, Func<UserContext, User, bool>[]> AdminValidations { get; } = new()
        {
            { Operation.CreateOperation, NoSpecificRules() },
            { Operation.UpdateOperation, NoSpecificRules() },
            { Operation.ReadOperation, NoSpecificRules() },
            { Operation.DeleteOperation, NoSpecificRules() }
        };

        protected override Dictionary<Operation, Func<UserContext, User, bool>[]> ManagerValidations { get; } = new()
        {
            { Operation.UpdateOperation, SpecificRules(IsOwner) },
            { Operation.ReadOperation, SpecificRules(IsOwner, IsSuperior) },
            { Operation.DeleteOperation, SpecificRules(IsOwner) }
        };

        protected override Dictionary<Operation, Func<UserContext, User, bool>[]> CustomerValidations { get; } = new()
        {
            { Operation.UpdateOperation, SpecificRules(IsOwner) },
            { Operation.ReadOperation, SpecificRules(IsOwner) },
            { Operation.DeleteOperation, SpecificRules(IsOwner) }
        };

        public UserValidations()
        {
            Rules = new()
            {
                { Role.Admin, AdminValidations },
                { Role.Manager, ManagerValidations },
                { Role.Customer, CustomerValidations },
            };
        }

        /// <summary>
        /// Check is user owner of resource
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private static bool IsOwner(UserContext userContext, User resource) => userContext.UserId == resource.Id;

        /// <summary>
        /// Check is user superiors of resource
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private static bool IsSuperior(UserContext userContext, User resource) => userContext.Role < resource.Identity.Role;
    }
}

