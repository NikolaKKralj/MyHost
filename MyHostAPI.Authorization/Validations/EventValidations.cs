using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Models;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain;

namespace MyHostAPI.Authorization.Validations
{
    public class EventValidations : BaseValidations<Event>, IValidations<Event>
    {
        public override Dictionary<Role, Dictionary<Operation, Func<UserContext, Event, bool>[]>> Rules { get; }

        protected override Dictionary<Operation, Func<UserContext, Event, bool>[]> AdminValidations { get; } = new()
        {
            { Operation.CreateOperation, NoSpecificRules() },
            { Operation.ReadOperation, NoSpecificRules() },
            { Operation.UpdateOperation, NoSpecificRules() },
            { Operation.DeleteOperation, NoSpecificRules() }
        };

        protected override Dictionary<Operation, Func<UserContext, Event, bool>[]> ManagerValidations { get; } = new()
        {
            { Operation.CreateOperation, NoSpecificRules() },
            { Operation.ReadOperation, NoSpecificRules() },
            { Operation.UpdateOperation, NoSpecificRules() },
            { Operation.DeleteOperation, NoSpecificRules() }
        };

        protected override Dictionary<Operation, Func<UserContext, Event, bool>[]> CustomerValidations { get; } = new()
        {
            { Operation.ReadOperation, NoSpecificRules() }
        };

        public EventValidations()
        {
            Rules = new()
            {
                { Role.Admin, AdminValidations },
                { Role.Manager, ManagerValidations },
                { Role.Customer, CustomerValidations }
            };
        }                    
    }
}
