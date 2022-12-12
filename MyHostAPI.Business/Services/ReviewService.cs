using AutoMapper;
using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain;
using MyHostAPI.Models;
using static MyHostAPI.Data.Specifications.PremiseSpecification;
using static MyHostAPI.Data.Specifications.ReviewSpecification;

namespace MyHostAPI.Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationHandler<Review> _authorizationHandler;
        private readonly IPremiseRepository _premiseRepository;


        public ReviewService(IReviewRepository reviewRepository,
            IMapper mapper,
            IAuthorizationHandler<Review> authorizationHandler,
            IPremiseRepository premiseRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _authorizationHandler = authorizationHandler;
            _premiseRepository = premiseRepository;
        }

        public async Task CreateReview(ReviewModel reviewModel, UserContext userContext)
        {
            var review = _mapper.Map<Review>(reviewModel);

            await _authorizationHandler.Authorize(userContext, review, Operation.CreateOperation);

            await _reviewRepository.CreateAsync(review);

            await CalculatePremiseAverage(reviewModel.PremiseId);
        }

        public async Task DeleteReview(string id, UserContext userContext)
        {
            var review = await _reviewRepository.FindOneByAsync(new ReviewById(id));

            review.IsDeleted = true;

            await _authorizationHandler.Authorize(userContext, review, Operation.DeleteOperation);

            await _reviewRepository.UpdateAsync(review);

            await CalculatePremiseAverage(review.PremiseId);
        }

        public async Task<PaginatedList<ReviewModel>> GetPremiseReview(string premiseid, Pagination pagination, UserContext userContext)
        {
            var review = await _reviewRepository.FindManyByAsync(new ReviewByPremiseId(premiseid), pagination);

            review.ForEach(async x => await _authorizationHandler.Authorize(userContext, x, Operation.ReadOperation));

            var reviewModel = _mapper.Map<PaginatedList<ReviewModel>>(review);

            return reviewModel;
        }

        public async Task UpdateReview(ReviewUpdateModel reviewModel, UserContext userContext)
        {
            var review = await _reviewRepository.FindOneByAsync(new ReviewById(reviewModel.Id));

            var mappedReview = _mapper.Map(reviewModel, review);

            await _authorizationHandler.Authorize(userContext, mappedReview, Operation.UpdateOperation);

            await _reviewRepository.UpdateAsync(mappedReview);

            await CalculatePremiseAverage(review.PremiseId);
        }

        private async Task CalculatePremiseAverage(string premiseId)
        {
            var premiseReviews = await _reviewRepository.FindManyByAsync(new ReviewByPremiseId(premiseId));

            var ratingAverage = Math.Round(premiseReviews.Average(x => x.Rating), 2);

            var selectedPremise = await _premiseRepository.FindOneByAsync(new PremiseById(premiseId));

            selectedPremise.RatingAverage = ratingAverage;

            await _premiseRepository.UpdateAsync(selectedPremise);
        }
    }
}
