using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain;

namespace MyHostAPI.Data.Repositories
{
    public class OperatingCityRepository : GenericRepository<OperatingCity>, IOperatingCityRepository
    {
        public OperatingCityRepository(IMongoClient mongoClient, IConfiguration configuration)
            : base(mongoClient, configuration)
        {

        }
    }
}
