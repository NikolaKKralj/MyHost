using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain;

namespace MyHostAPI.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IMongoClient mongoClient, IConfiguration configuration)
            : base(mongoClient, configuration)
        {
        }
    }
}

