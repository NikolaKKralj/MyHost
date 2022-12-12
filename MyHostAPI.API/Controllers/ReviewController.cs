using Microsoft.AspNetCore.Mvc;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Attributes;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Domain;
using MyHostAPI.Models;

namespace MyHostAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize(Role.Admin, Role.Manager, Role.Customer)]
        [HttpGet]
        [Route("premise/{premiseId}")]
        public async Task<IActionResult> GetPremiseReview([FromRoute] string premiseId, [FromQuery] Pagination pagination)
        {
            var review = await _reviewService.GetPremiseReview(premiseId, pagination, UserContext);

            this.SetPaginationData(review.Metadata);
            return Ok(review);
        }

        [Authorize(Role.Customer, Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewModel reviewModel)
        {
            await _reviewService.CreateReview(reviewModel, UserContext);
            return Ok();
        }

        [Authorize(Role.Customer, Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            await _reviewService.DeleteReview(id, UserContext);
            return Ok();
        }

        [Authorize(Role.Customer, Role.Admin)]
        [HttpPut]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewUpdateModel reviewModel)
        {
            await _reviewService.UpdateReview(reviewModel, UserContext);
            return Ok();
        }

     }
}
