using MyHostAPI.Domain;

namespace MyHostAPI.Data.Specifications
{
    public class ReviewSpecification
    {
        public class ReviewById : BaseSpecification<Review>
        {
            public ReviewById(string id) : base (r => r.Id == id && r.IsDeleted == false)
            {
            }
        }
        public class ReviewByPremiseId : BaseSpecification<Review>
        {
            public ReviewByPremiseId(string id) : base (r => r.PremiseId == id && r.IsDeleted == false)
            {
            }
        }
        public class ActiveReviews : BaseSpecification<Review>
        {
            public ActiveReviews() : base (r => r.IsDeleted == false)
            {
            }
        }
    }
}
