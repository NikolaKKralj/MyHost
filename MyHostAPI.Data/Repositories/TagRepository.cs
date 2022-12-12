using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Data.Interfaces;

namespace MyHostAPI.Data.Repositories
{
    public class TagRepository : GenericRepository<Domain.Tag>, ITagRepository
    {
        public TagRepository(IMongoClient mongoClient, IConfiguration configuration) 
            : base(mongoClient, configuration)
        {
        }
    }
}
