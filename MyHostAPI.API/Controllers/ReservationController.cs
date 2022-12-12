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
    public class ReservationController : BaseController
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllReservations([FromQuery] Pagination pagination)
        {
            var reservations = await _reservationService.GetAllReservations(pagination, UserContext);

            this.SetPaginationData(reservations.Metadata);
            return Ok(reservations);
        }

        [Authorize(Role.Manager, Role.Customer)]
        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetUserReservations([FromRoute] string id, [FromQuery] Pagination pagination)
        {
            var reservations = await _reservationService.GetUserReservations(id, pagination, UserContext);

            this.SetPaginationData(reservations.Metadata);
            return Ok(reservations);
        }


        [Authorize(Role.Admin, Role.Manager, Role.Customer)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(string id)
        {
            var reservation = await _reservationService.GetReservationById(id, UserContext);
            return Ok(reservation);
        }

        [Authorize(Role.Admin, Role.Manager, Role.Customer)]
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationModel reservationModel)
        {
            await _reservationService.CreateReservation(reservationModel, UserContext);
            return Ok();
        }

        [Authorize(Role.Admin, Role.Manager)]
        [HttpPut]
        [Route("manager")]
        public async Task<IActionResult> ManagerUpdateReservation([FromBody] ReservationManagerUpdateModel reservationModel)
        {
            await _reservationService.ManagerUpdateReservation(reservationModel, UserContext);
            return Ok();
        }

        [Authorize(Role.Customer)]
        [HttpPut]
        [Route("customer")]
        public async Task<IActionResult> CustomerUpdateReservation([FromBody] ReservationCustomerUpdateModel reservationModel)
        {
            await _reservationService.CustomerUpdateReservation(reservationModel, UserContext);
            return Ok();
        }

        [Authorize(Role.Admin, Role.Manager, Role.Customer)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(string id)
        {
            await _reservationService.DeleteReservation(id, UserContext);
            return Ok();
        }
    }
}
