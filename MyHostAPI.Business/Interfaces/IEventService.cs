using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Models;

namespace MyHostAPI.Business.Interfaces
{
    public interface IEventService
    {
        Task<PaginatedList<EventModel>> GetAll(Pagination pagination, UserContext userContext);
        Task<PaginatedList<EventModel>> GetPremiseEvents(string premiseid, Pagination pagination, UserContext userContext);
        Task<EventModel> GetEventById(string id, UserContext userContext);
        Task CreateEvent(EventModel eventModel, UserContext userContext);
        Task UpdateEvent(EventUpdateModel eventUpdateModel, UserContext userContext);
        Task DeleteEvent(string id, UserContext userContext);
    }
}
