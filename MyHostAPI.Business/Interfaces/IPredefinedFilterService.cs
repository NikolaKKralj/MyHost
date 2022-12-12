using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Models.PredefinedFilter;

namespace MyHostAPI.Business.Interfaces
{
    public interface IPredefinedFilterService
    {
        Task<PaginatedList<PredefinedFilterModel>> GetAllAsync(Pagination pagination, UserContext userContext);
        Task<PredefinedFilterModel> GetByIdAsync(string id, UserContext userContext);
        Task CreateAsync(PredefinedFilterModel predefinedFilterModel, UserContext userContext);
        Task UpdateAsync(PredefinedFilterModel predefinedFilterModel, UserContext userContext);
        Task DeleteAsync(string id, UserContext userContext);
    }
}
