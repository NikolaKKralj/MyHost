using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Models;
using MyHostAPI.Domain;
using MyHostAPI.Domain.Premise;

namespace MyHostAPI.Authorization.Validations
{
    public class PremiseValidations : BaseValidations<Premise>, IValidations<Premise>
    {

        public override Dictionary<Role, Dictionary<Operation, Func<UserContext, Premise, bool>[]>> Rules { get; }

        protected override Dictionary<Operation, Func<UserContext, Premise, bool>[]> AdminValidations { get; } = new()
        {
            { Operation.CreateOperation, NoSpecificRules() },
            { Operation.UpdateOperation, NoSpecificRules() },
            { Operation.ReadOperation, NoSpecificRules() },
            { Operation.DeleteOperation, NoSpecificRules() }
        };

        protected override Dictionary<Operation, Func<UserContext, Premise, bool>[]> ManagerValidations { get; } = new()
        {
            { Operation.UpdateOperation, SpecificRules(IsOwner) },
            { Operation.ReadOperation, SpecificRules(IsOwner) },
        };

        protected override Dictionary<Operation, Func<UserContext, Premise, bool>[]> CustomerValidations { get; } = new()
        {
            { Operation.ReadOperation, NoSpecificRules() },
        };

        public PremiseValidations()
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
        private static bool IsOwner(UserContext userContext, Premise resource) => userContext.UserId == resource.ManagerId;
    }
}

