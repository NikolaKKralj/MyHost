using MyHostAPI.Domain.PredefinedFilter;

namespace MyHostAPI.Data.Specifications
{
    public class PredefinedFilterSpecification
    {
        public class ActivePredefinedFilters : BaseSpecification<PredefinedFilter>
        {
            public ActivePredefinedFilters() : base(x => x.IsDeleted == false)
            {
            }
        }
    }
}
