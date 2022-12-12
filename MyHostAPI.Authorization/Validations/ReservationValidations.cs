using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Models;
using MyHostAPI.Domain;

namespace MyHostAPI.Authorization.Validations
{
    public class ReservationValidations : BaseValidations<Reservation>, IValidations<Reservation>
    {
        public override Dictionary<Role, Dictionary<Operation, Func<UserContext, Reservation, bool>[]>> Rules { get; }

        protected override Dictionary<Operation, Func<UserContext, Reservation, bool>[]> AdminValidations { get; } = new()
        {
            { Operation.CreateOperation, NoSpecificRules() },
            { Operation.UpdateOperation, NoSpecificRules() },
            { Operation.ReadOperation, NoSpecificRules() },
            { Operation.DeleteOperation, NoSpecificRules() }
        };
        protected override Dictionary<Operation, Func<UserContext, Reservation, bool>[]> ManagerValidations { get; } = new()
        {
            { Operation.CreateOperation, SpecificRules(IsSuperior) },
            { Operation.UpdateOperation, SpecificRules(IsSuperior, NotFinishedStatus) },
            { Operation.ReadOperation, SpecificRules(IsSuperior) },
            { Operation.DeleteOperation, SpecificRules(IsSuperior) }
        };

        protected override Dictionary<Operation, Func<UserContext, Reservation, bool>[]> CustomerValidations { get; } = new()
        {
            { Operation.CreateOperation, SpecificRules(IsOwner) },
            { Operation.UpdateOperation, SpecificRules(IsOwner, RequestedStatus) },
            { Operation.ReadOperation, SpecificRules(IsOwner) },
            { Operation.DeleteOperation, SpecificRules(IsOwner, RequestedStatus) }
        };

        public ReservationValidations()
        {
            Rules = new()
            {
                { Role.Admin, AdminValidations },
                { Role.Manager, ManagerValidations },
                { Role.Customer, CustomerValidations },
            };
        }

        /// <summary>
        /// Check is manager owner of reservation
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        private static bool IsSuperior(UserContext userContext, Reservation reservation) => userContext.UserId == reservation.ManagerId;

        /// <summary>
        /// Check is customer owner of reservation
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        private static bool IsOwner(UserContext userContext, Reservation reservation) => userContext.UserId == reservation.CustomerId;

        /// <summary>
        /// Customer check is update allowed
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>
        private static bool RequestedStatus(UserContext userContext, Reservation reservation) => reservation.Status == Status.Requested;

        /// <summary>
        /// Manager check is update allowed
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="reservation"></param>
        /// <returns></returns>    
        private static bool NotFinishedStatus(UserContext userContext, Reservation reservation) => reservation.Status != Status.Finished;
    }
}
