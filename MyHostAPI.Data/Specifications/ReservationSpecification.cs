using MyHostAPI.Domain;

namespace MyHostAPI.Data.Specifications
{
    public class ReservationSpecification
    {
        public class ActiveReservation : BaseSpecification<Reservation>
        {
            public ActiveReservation() : base(r => r.IsDeleted == false)
            {
            }
        }

        public class ReservationById : BaseSpecification<Reservation>
        {
            public ReservationById(string id) : base(r => r.Id == id && r.IsDeleted == false)
            {
            }
        }

        public class ActiveReservationByManager : BaseSpecification<Reservation>
        {
            public ActiveReservationByManager(string managerId) : base(r => r.ManagerId == managerId && r.IsDeleted == false)
            {
            }
        }

        public class ActiveReservationByCustomer : BaseSpecification<Reservation>
        {
            public ActiveReservationByCustomer(string customerId) : base(r => r.CustomerId == customerId && r.IsDeleted == false)
            {
            }
        }

    }
}

