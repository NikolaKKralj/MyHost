using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Models;

namespace MyHostAPI.Business.Interfaces
{
    public interface IReviewService
    {
        Task<PaginatedList<ReviewModel>> GetPremiseReview(string premiseid, Pagination pagination, UserContext userContext);
        Task CreateReview(ReviewModel reviewModel, UserContext userContext);
        Task UpdateReview(ReviewUpdateModel reviewModel, UserContext userContext);
        Task DeleteReview(string id, UserContext userContext);
    }
}
