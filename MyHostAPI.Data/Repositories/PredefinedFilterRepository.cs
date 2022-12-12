using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain.PredefinedFilter;

namespace MyHostAPI.Data.Repositories
{
    public class PredefinedFilterRepository : GenericRepository<PredefinedFilter>, IPredefinedFilterRepository
    {
        public PredefinedFilterRepository(IMongoClient mongoClient, IConfiguration configuration)
            : base(mongoClient, configuration)
        {
        }
    }
}
