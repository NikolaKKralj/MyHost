using Microsoft.AspNetCore.Mvc;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Attributes;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Domain;
using MyHostAPI.Models.PredefinedFilter;

namespace MyHostAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredefinedFilterController : BaseController
    {
        private readonly IPredefinedFilterService _predefinedFilterService;

        public PredefinedFilterController(IPredefinedFilterService predefinedFilterService)
        {
            _predefinedFilterService = predefinedFilterService;
        }

        [Authorize(Role.Admin, Role.Customer, Role.Manager)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            var predefinedFilters = await _predefinedFilterService.GetAllAsync(pagination, UserContext);

            SetPaginationData(predefinedFilters.Metadata);
            return Ok(predefinedFilters);
        }

        [Authorize(Role.Admin, Role.Customer, Role.Manager)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var predefinedFilter = await _predefinedFilterService.GetByIdAsync(id, UserContext);

            return Ok(predefinedFilter);
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreatePredefinedFilter([FromBody] PredefinedFilterModel predefinedFilterModel)
        {
            await _predefinedFilterService.CreateAsync(predefinedFilterModel, UserContext);

            return Ok();
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        public async Task<IActionResult> UpdatePredefinedFilter([FromBody] PredefinedFilterModel predefinedFilterModel)
        {
            await _predefinedFilterService.UpdateAsync(predefinedFilterModel, UserContext);

            return Ok();
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _predefinedFilterService.DeleteAsync(id, UserContext);

            return Ok();
        }
    }
}
