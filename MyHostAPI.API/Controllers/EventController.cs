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
    public class EventController : BaseController
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Authorize(Role.Admin, Role.Manager, Role.Customer)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            var events = await _eventService.GetAll(pagination, UserContext);

            this.SetPaginationData(events.Metadata);
            return Ok(events);
        }

        [Authorize(Role.Admin, Role.Manager, Role.Customer)]
        [HttpGet]
        [Route("premise/{premiseid}")]
        public async Task<IActionResult> GetPremiseEvents([FromRoute] string premiseid, [FromQuery] Pagination pagination)
        {
            var events = await _eventService.GetPremiseEvents(premiseid, pagination, UserContext);

            this.SetPaginationData(events.Metadata);
            return Ok(events);
        }

        [Authorize(Role.Admin, Role.Manager, Role.Customer)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(string id)
        {
            var selectedEvent = await _eventService.GetEventById(id, UserContext);

            return Ok(selectedEvent);
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventModel eventModel)
        {
            await _eventService.CreateEvent(eventModel, UserContext);
            return Ok();
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPut]
        public async Task<IActionResult> UpdateEvent([FromBody] EventUpdateModel eventUpdateModel)
        {
            await _eventService.UpdateEvent(eventUpdateModel, UserContext);
            return Ok();
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            await _eventService.DeleteEvent(id, UserContext);
            return Ok();
        }

    }
}
