using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain;

namespace MyHostAPI.Data.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(IMongoClient mongoClient, IConfiguration configuration) 
            : base(mongoClient, configuration)
        {
        }
    }
}
