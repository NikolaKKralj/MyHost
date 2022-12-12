using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Models;

namespace MyHostAPI.Business.Interfaces
{
    public interface IReservationService
    {
        Task<PaginatedList<ReservationModel>> GetAllReservations(Pagination pagination, UserContext userContext);
        Task<PaginatedList<ReservationModel>> GetUserReservations(string id, Pagination pagination, UserContext userContext);
        Task<ReservationModel> GetReservationById(string id, UserContext userContext);
        Task CreateReservation(ReservationModel reservationModel, UserContext userContext);
        Task CustomerUpdateReservation(ReservationCustomerUpdateModel reservationModel, UserContext userContext);
        Task ManagerUpdateReservation(ReservationManagerUpdateModel reservationModel, UserContext userContext);
        Task DeleteReservation(string id, UserContext userContext);

    }
}
