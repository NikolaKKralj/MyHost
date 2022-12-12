using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Models;
using MyHostAPI.Domain;

namespace MyHostAPI.Authorization.Validations
{
    public class ReviewValidations : BaseValidations<Review>, IValidations<Review>
    {
        public override Dictionary<Role, Dictionary<Operation, Func<UserContext, Review, bool>[]>> Rules { get; }

        protected override Dictionary<Operation, Func<UserContext, Review, bool>[]> AdminValidations { get; } = new ()
        {
            { Operation.CreateOperation, NoSpecificRules() },
            { Operation.ReadOperation, NoSpecificRules() },
            { Operation.UpdateOperation, NoSpecificRules() },
            { Operation.DeleteOperation, NoSpecificRules() }
        };

        protected override Dictionary<Operation, Func<UserContext, Review, bool>[]> ManagerValidations { get; } = new()
        {
            { Operation.ReadOperation, NoSpecificRules() }
        };

        protected override Dictionary<Operation, Func<UserContext, Review, bool>[]> CustomerValidations { get; } = new()
        {
            { Operation.CreateOperation, NoSpecificRules() },
            { Operation.ReadOperation, NoSpecificRules() },
            { Operation.UpdateOperation, SpecificRules(IsOwner) },
            { Operation.DeleteOperation, SpecificRules(IsOwner) }
        };

        public ReviewValidations()
        {
            Rules = new()
            {
                { Role.Admin, AdminValidations },
                { Role.Manager, ManagerValidations },
                { Role.Customer, CustomerValidations },
            };
        }

        /// <summary>
        /// Check is customer owner of reservation
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        private static bool IsOwner(UserContext userContext, Review review) => userContext.UserId == review.CustomerId;

    }
}
