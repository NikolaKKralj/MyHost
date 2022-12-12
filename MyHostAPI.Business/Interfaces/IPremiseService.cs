using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Models;
using MyHostAPI.Models.Premise;

namespace MyHostAPI.Business.Interfaces
{
    public interface IPremiseService
    {
        Task<PremiseResponseModel> CreateAsync(PremiseModel premiseModel, UserContext userContext);
        Task<PaginatedList<PremiseResponseModel>> GetAllAsync(Pagination pagination);
        Task<PremiseResponseModel> GetAsync(string id);
        Task<PremiseResponseModel> UpdateAsync(PremiseModel premiseModel, UserContext userContext);
        Task DeleteAsync(string id, UserContext userContext);
        Task DeleteMenuItemAsync(string image, UserContext userContext);
        Task <PaginatedList<PremiseResponseModel>> PremiseSearch(PremiseSearchModel searchModel, Pagination pagination);
        Task<PaginatedList<PremiseTypeModel>> GetPremiseTypes();
        Task<IEnumerable<TagModel>> GetPremiseTags();

    }
}
