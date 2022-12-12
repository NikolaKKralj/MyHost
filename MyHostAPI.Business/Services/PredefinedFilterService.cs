using AutoMapper;
using Microsoft.Extensions.Logging;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Exceptions;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain.PredefinedFilter;
using MyHostAPI.Models.PredefinedFilter;
using static MyHostAPI.Data.Specifications.PredefinedFilterSpecification;

namespace MyHostAPI.Business.Services
{
    public class PredefinedFilterService : IPredefinedFilterService
    {
        private readonly IPredefinedFilterRepository _predefinedFilterRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PredefinedFilterService> _logger;

        public PredefinedFilterService(IPredefinedFilterRepository predefinedFilterRepository,
            IMapper mapper,
            ILogger<PredefinedFilterService> logger)
        {
            _predefinedFilterRepository = predefinedFilterRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateAsync(PredefinedFilterModel predefinedFilterModel, UserContext userContext)
        {
            var mappedPredefinedFilter = _mapper.Map<PredefinedFilter>(predefinedFilterModel);

            await _predefinedFilterRepository.CreateAsync(mappedPredefinedFilter);
        }

        public async Task DeleteAsync(string id, UserContext userContext)
        {
            var predefinedFilter = await _predefinedFilterRepository.GetAsync(id);

            predefinedFilter.IsDeleted = true;

            await _predefinedFilterRepository.UpdateAsync(predefinedFilter);
        }

        public async Task<PaginatedList<PredefinedFilterModel>> GetAllAsync(Pagination pagination, UserContext userContext)
        {
            var predefinedFilters = await _predefinedFilterRepository.FindManyByAsync(new ActivePredefinedFilters(), pagination);

            return _mapper.Map<PaginatedList<PredefinedFilterModel>>(predefinedFilters);
        }

        public async Task<PredefinedFilterModel> GetByIdAsync(string id, UserContext userContext)
        {
            var predefinedFilter = await _predefinedFilterRepository.GetAsync(id);

            return _mapper.Map<PredefinedFilterModel>(predefinedFilter);
        }

        public async Task UpdateAsync(PredefinedFilterModel predefinedFilterModel, UserContext userContext)
        {
            if (predefinedFilterModel.Id == null)
            {
                _logger.LogError($"Predefined Filter id is null!");
                throw new RecordNotFoundException($"Predefined Filter id is null!");
            }
            var existingPredefinedFilter = await _predefinedFilterRepository.GetAsync(predefinedFilterModel.Id);

            var mappedPredefinedFilter = _mapper.Map(predefinedFilterModel, existingPredefinedFilter);

            await _predefinedFilterRepository.UpdateAsync(mappedPredefinedFilter);
        }
    }
}
