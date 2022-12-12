using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Data.Specifications;
using MyHostAPI.Domain.PredefinedFilter;
using MyHostAPI.Domain.Premise;
using MyHostAPI.Models;
using MyHostAPI.Models.Premise;
using System.Linq.Expressions;
using static MyHostAPI.Common.Helpers.ToLambdaExpression;

namespace MyHostAPI.Data.Repositories
{
    public class PremiseRepository : GenericRepository<Premise>, IPremiseRepository
    {
        private const int _minRating = 1;
        private const int _maxRating = 5;
        private const double _deltaRating = 0.5;
        private readonly IMongoCollection<Domain.Tag> tagCollection;
        private readonly IMongoCollection<PredefinedFilter> predefinedFilterCollection;
        private readonly IMongoCollection<PremiseType> premiseTypeCollection;

        public PremiseRepository(IMongoClient mongoClient, IConfiguration configuration)
            : base(mongoClient, configuration)
        {
            tagCollection = mongoDatabase.GetCollection<Domain.Tag>(typeof(Domain.Tag).Name);
            predefinedFilterCollection = mongoDatabase.GetCollection<PredefinedFilter>(typeof(PredefinedFilter).Name);
            premiseTypeCollection = mongoDatabase.GetCollection<PremiseType>(typeof(PremiseType).Name);
        }

        public async Task<PaginatedList<Premise>> PremiseSearch(PremiseSearchModel premiseSearchModel, Pagination pagination)
        {
            pagination ??= new Pagination();
            var source = entityCollection.AsQueryable();

            // find premise type
            PremiseType premiseType = new();
            if (!string.IsNullOrWhiteSpace(premiseSearchModel.PremiseTypeId))
            {
                var premiseTypes = await premiseTypeCollection.FindAsync(x => !x.IsDeleted && x.Id == premiseSearchModel.PremiseTypeId);
                premiseType = (await premiseTypes.ToListAsync()).FirstOrDefault() ?? new();
            }

            // find all selected predefined filters
            List<string> predefinedFilters = new();
            var matchedPredefinedFilterPremises = await predefinedFilterCollection.Find(x => premiseSearchModel.PredefinedFiltersId.Contains(x.Id)).ToListAsync();
            matchedPredefinedFilterPremises.ForEach(x => predefinedFilters.AddRange(x.PremiseIds));

            // find tag
            Domain.Tag  tag = new();
            if (!string.IsNullOrWhiteSpace(premiseSearchModel.SearchInput))
            {
               var tagSource = await tagCollection.FindAsync(x => !x.IsDeleted && x.Name == premiseSearchModel.SearchInput.ToLower());
               tag = (await tagSource.ToListAsync()).FirstOrDefault() ?? new();
            }

            source = BuildQuery(source, premiseSearchModel, tag, predefinedFilters, premiseType);
               
            var result = source.Skip((pagination.PageIndex) * pagination.PageSize).Take(pagination.PageSize).ToList();
            var totalCount = source.Count();
            var totalPages = (long)Math.Ceiling(totalCount / (double)pagination.PageSize);

            var paginationData = new PaginatedListMetadata()
            {
                TotalPages = totalPages,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                HasPreviousPage = pagination.PageIndex > 1,
                HasNextPage = pagination.PageIndex < totalPages
            };

            return new PaginatedList<Premise>(result, paginationData);
        }


        private static IMongoQueryable<Premise> BuildQuery(IMongoQueryable<Premise> source, PremiseSearchModel premiseSearchModel, Domain.Tag tag, List<string> predefinedFilters, PremiseType premiseType)
        {
            Dictionary<Func<bool>, Expression<Func<Premise, bool>>> searchEngine = new()
            {
                // Search premises by premise type
                {
                    () => !string.IsNullOrWhiteSpace(premiseSearchModel.PremiseTypeId),
                    premise => premiseType.PremiseIds.Contains(premise.Id)
                },
                // Search premises by predefined filter
                {
                    () => predefinedFilters is not null && predefinedFilters.Count > 0,
                    premise => predefinedFilters.Contains(premise.Id) && !premise.IsDeleted
                },
                // Search by rating
                {
                    () => premiseSearchModel.Rating >= _minRating && premiseSearchModel.Rating <= _maxRating,
                    premise => premise.RatingAverage < premiseSearchModel.Rating + _deltaRating && premise.RatingAverage >= + _deltaRating && !premise.IsDeleted
                },
                // Search by input or tag
                {
                    () => !string.IsNullOrWhiteSpace(premiseSearchModel.SearchInput),
                    premise => !premise.IsDeleted && (premise.Name.ToLower().Contains(premiseSearchModel.SearchInput.ToLower()) || tag.PremiseIds.Contains(premise.Id))
                },
                // Search by operating city
                {
                    () => !string.IsNullOrWhiteSpace(premiseSearchModel.OperatingCity),
                    premise => premise.Location.City.ToLower() == premiseSearchModel.OperatingCity.ToLower()
                }
            };

            foreach(var filter in searchEngine)
            {
                if (filter.Key.Invoke())
                {
                    source = source.Where(filter.Value);
                }
            }

            source = premiseSearchModel.OrderByDescending == true ? source.OrderByDescending(ToLambda<Premise>(premiseSearchModel.SortBy.ToString())) : source.OrderBy(ToLambda<Premise>(premiseSearchModel.SortBy.ToString()));

            source.DistinctBy(source => source.Id);

            return source;
        }
    }
}

