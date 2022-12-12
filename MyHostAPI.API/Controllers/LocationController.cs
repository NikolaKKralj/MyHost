using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Attributes;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Models;

namespace MyHostAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : BaseController
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("cities")]
        public async Task<IActionResult> GetOperatingCities()
        {
            var operatingCities = await _locationService.GetOperatingCities();

            this.SetPaginationData(operatingCities.Metadata);
            return Ok(operatingCities);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("cities/user-location")]
        public async Task<IActionResult> GetOperatingCityByUserLocation([FromQuery] LocationModel locationModel)
        {
            var operatingCity = await _locationService.GetOperatingCityByUserLocation(locationModel);
            return Ok(operatingCity);
        }
    }
}
