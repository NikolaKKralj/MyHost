using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyHostAPI.Common.Configurations;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Data.Specifications;
using MyHostAPI.Domain;

namespace MyHostAPI.Data.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly IMongoDatabase mongoDatabase;
        protected readonly IMongoCollection<T> entityCollection;

        public GenericRepository(IMongoClient mongoClient, IConfiguration configuration)
        {
            var databaseSection = configuration.GetSection(DatabaseSection.Name).Get<DatabaseSection>();

            mongoDatabase = mongoClient.GetDatabase(databaseSection.DatabaseName);
            entityCollection = mongoDatabase.GetCollection<T>(typeof(T).Name);
        }

        public async Task CreateAsync(T newEntity) =>
            await entityCollection.InsertOneAsync(newEntity);


        public async Task<PaginatedList<T>> GetAllAsync(Pagination? pagination = null)
        {
            pagination ??= new Pagination();

            var source = entityCollection.Find(_ => true);
            var count = await source.CountDocumentsAsync();
            var totalPages = (long)Math.Ceiling(count / (double)pagination.PageSize);
            var result = await source.Skip((pagination.PageIndex) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync();

            var paginationData = new PaginatedListMetadata()
            {
                TotalPages = totalPages,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                HasPreviousPage = pagination.PageIndex > 1,
                HasNextPage = pagination.PageIndex < totalPages
            };

            var paginatedList = new PaginatedList<T>(result, paginationData);
            return paginatedList;
        }

        public async Task<List<T>> FindManyByAsync(ISpecification<T> specification) =>
            await entityCollection.Find(specification.Criteria).ToListAsync();


        public async Task<T> FindOneByAsync(ISpecification<T> specification) =>
            await entityCollection.Find(specification.Criteria).FirstOrDefaultAsync();

        public async Task<PaginatedList<T>> FindManyByAsync(ISpecification<T> specification, Pagination pagination)
        {
            pagination ??= new Pagination();

            var source = entityCollection.Find(specification.Criteria);
            var count = await source.CountDocumentsAsync();
            var totalPages = (long)Math.Ceiling(count / (double)pagination.PageSize);
            var result = await source.Skip((pagination.PageIndex) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync();

            var paginationData = new PaginatedListMetadata()
            {
                TotalPages = totalPages,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                HasPreviousPage = pagination.PageIndex > 1,
                HasNextPage = pagination.PageIndex < totalPages
            };

            var paginatedList = new PaginatedList<T>(result, paginationData);
            return paginatedList;
        }

        public async Task<T> GetAsync(string id) =>
            await entityCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task RemoveAsync(string id) =>
            await entityCollection.DeleteOneAsync(x => x.Id == id);

        public async Task UpdateAsync(T updatedEntity) =>
            await entityCollection.ReplaceOneAsync(x => x.Id == updatedEntity.Id, updatedEntity);
    }
}

