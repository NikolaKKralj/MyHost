using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain;

namespace MyHostAPI.Data.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(IMongoClient mongoClient, IConfiguration configuration)
            : base(mongoClient, configuration)
        {
        }
    }
}
