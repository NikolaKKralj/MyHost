using MyHostAPI.Domain;

namespace MyHostAPI.Data.Specifications
{
    public class EventSpecification
    {
        public class EventById : BaseSpecification<Event>
        {
            public EventById(string id) : base(x => x.Id == id && x.IsDeleted == false)
            {
            }
        }
        public class ActiveEvent : BaseSpecification<Event>
        {
            public ActiveEvent() : base( x => x.IsDeleted == false)
            {
            }
        }
        public class EventsByPremiseId : BaseSpecification<Event>
        {
            public EventsByPremiseId(string id) : base (x => x.PremiseId == id && x.IsDeleted == false)
            {
            }
        }
    }
}
