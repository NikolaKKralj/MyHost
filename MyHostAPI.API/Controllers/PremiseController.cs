using Microsoft.AspNetCore.Mvc;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Attributes;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Domain;
using MyHostAPI.Domain.Premise;
using MyHostAPI.Models;
using MyHostAPI.Models.Premise;

namespace MyHostAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PremiseController : BaseController
    {
        private readonly IPremiseService _premiseService;

        public PremiseController(IPremiseService premiseService)
        {
            _premiseService = premiseService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPremises([FromQuery] Pagination pagination)
        {
            var premises = await _premiseService.GetAllAsync(pagination);

            this.SetPaginationData(premises.Metadata);
            return Ok(premises);
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreatePremise([FromForm] PremiseModel premiseModel)
        {
            await _premiseService.CreateAsync(premiseModel, UserContext);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPremise(string id)
        {
            var premise = await _premiseService.GetAsync(id);
            return Ok(premise);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPut]
        public async Task<IActionResult> UpdatePremise([FromForm] PremiseModel premiseModel)
        {
            await _premiseService.UpdateAsync(premiseModel, UserContext);
            return Ok();
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePremise(string id)
        {
            await _premiseService.DeleteAsync(id, UserContext);
            return Ok();
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpDelete]
        [Route("menu-item/{image}")]
        public async Task<IActionResult> DeleteMenuItem(string image)
        {
            await _premiseService.DeleteMenuItemAsync(image, UserContext);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchMethod([FromQuery] PremiseSearchModel premiseSearchModel, [FromQuery] Pagination pagination)
        {
            var premises = await _premiseService.PremiseSearch(premiseSearchModel, pagination);
            this.SetPaginationData(premises.Metadata);
            return Ok(premises);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("types")]
        public async Task<IActionResult> GetPremiseTypes()
        {
            var premiseTypes = await _premiseService.GetPremiseTypes();
            this.SetPaginationData(premiseTypes.Metadata);
            return Ok(premiseTypes);
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> GetPremiseTags()
        {
            var premiseTags = await _premiseService.GetPremiseTags();
            return Ok(premiseTags);
        }
    }
}
