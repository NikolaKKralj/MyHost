using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Data.Specifications;
using MyHostAPI.Domain;
using MyHostAPI.Domain.Premise;
using MyHostAPI.Models;
using MyHostAPI.Models.Premise;

namespace MyHostAPI.Data.Interfaces
{
    public interface IPremiseRepository : IGenericRepository<Premise>
    {
        Task<PaginatedList<Premise>> PremiseSearch(PremiseSearchModel premiseSearchModel, Pagination pagination);
    }
}

