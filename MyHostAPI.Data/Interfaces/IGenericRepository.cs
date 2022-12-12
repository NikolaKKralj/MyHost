using MyHostAPI.Common.Helpers;
using MyHostAPI.Data.Specifications;
using MyHostAPI.Domain;

namespace MyHostAPI.Data.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Get all entites async
        /// </summary>
        /// <returns></returns>
        Task<PaginatedList<T>> GetAllAsync(Pagination pagination);

        /// <summary>
        /// Get all entites without pagination
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<List<T>> FindManyByAsync(ISpecification<T> specification);

        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetAsync(string id);

        /// <summary>
        /// Create not-null new entity
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        Task CreateAsync(T newEntity);

        /// <summary>
        /// Update existing not-null entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedEntity"></param>
        /// <returns></returns>
        Task UpdateAsync(T updatedEntity);

        /// <summary>
        /// Remove entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveAsync(string id);

        /// <summary>
        /// Find one entity by specification
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        Task<T> FindOneByAsync(ISpecification<T> specification);

        /// <summary>
        /// Find many entites by specification
        /// </summary>
        /// <param name="specification"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        Task<PaginatedList<T>> FindManyByAsync(ISpecification<T> specification, Pagination pagination);
    }
}

