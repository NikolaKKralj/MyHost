using AutoMapper;
using Microsoft.Extensions.Logging;
using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Exceptions;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Domain;
using MyHostAPI.Domain.Premise;
using MyHostAPI.Models;
using MyHostAPI.Models.Premise;
using static MyHostAPI.Data.Specifications.PremiseSpecification;
using static MyHostAPI.Data.Specifications.PremiseTypeSpecification;
using static MyHostAPI.Data.Specifications.ReviewSpecification;
using static MyHostAPI.Data.Specifications.TagSpecification;

namespace MyHostAPI.Business.Services
{
    public class PremiseService : IPremiseService
    {
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly IPremiseRepository _premiseRepository;
        private readonly ILogger<PremiseService> _logger;
        private readonly IAuthorizationHandler<Premise> _authorizationHandler;
        private readonly ILocationService _locationService;
        private readonly IReviewRepository _reviewRepository;
        private readonly IPredefinedFilterRepository _predefinedFilterRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IPremiseTypeRepository _premiseTypeRepository;

        public PremiseService(IMediaService mediaService,
            IMapper mapper, IPremiseRepository premiseRepository,
            ILogger<PremiseService> logger,
            IAuthorizationHandler<Premise> authorizationHandler,
            ILocationService locationService,
            IReviewRepository reviewRepository,
            IPredefinedFilterRepository predefinedFilterRepository,
            ITagRepository tagRepository,
            IPremiseTypeRepository premiseTypeRepository)
        {
            _mediaService = mediaService;
            _mapper = mapper;
            _premiseRepository = premiseRepository;
            _logger = logger;
            _authorizationHandler = authorizationHandler;
            _locationService = locationService;
            _reviewRepository = reviewRepository;
            _predefinedFilterRepository = predefinedFilterRepository;
            _tagRepository = tagRepository;
            _premiseTypeRepository = premiseTypeRepository;
        }

        public async Task<PaginatedList<PremiseResponseModel>> GetAllAsync(Pagination pagination)
        {
            var premises = await _premiseRepository.FindManyByAsync(new ActivePremises(), pagination);

            var premisesModel = _mapper.Map<PaginatedList<PremiseResponseModel>>(premises);

            var premiseTypes = await _premiseTypeRepository.GetAllAsync(new());

            foreach (var premise in premisesModel)
            {
                var premiseType = premiseTypes.First(x => x.PremiseIds.Contains(premise.Id));

                premise.PremiseTypeName = premiseType.Name;
            }    

            return premisesModel;
        }

        public async Task<PremiseResponseModel> CreateAsync(PremiseModel premiseModel, UserContext userContext)
        {
            foreach (var menuItem in premiseModel.MenuItems)
            {
                if (menuItem.ImageFile != null)
                {
                    var menuItemImageUri = await _mediaService.UploadImage(menuItem.ImageFile);
                    menuItem.Path = menuItemImageUri;
                }
            }

            foreach (var image in premiseModel.Images)
            {
                if (image.ImageFile != null)
                {
                    var imageUri = await _mediaService.UploadImage(image.ImageFile);
                    image.Path = imageUri;
                }
            }

            var premise = _mapper.Map<Premise>(premiseModel);
  
            await _authorizationHandler.Authorize(userContext, premise, Operation.CreateOperation);

            await _locationService.AddOperatingCity(premiseModel.Location.City);

            await _premiseRepository.CreateAsync(premise);

            foreach (var predefinedFilterId in premiseModel.PredefinedFilters)
            {
                var predefinedFilterDb = await _predefinedFilterRepository.GetAsync(predefinedFilterId);

                predefinedFilterDb.PremiseIds.Add(premise.Id);

                _logger.LogInformation("Premise Id successfully added into predefined filters table.");

                await _predefinedFilterRepository.UpdateAsync(predefinedFilterDb);
            }

            var premiseType = await _premiseTypeRepository.FindOneByAsync(new PremiseTypeById(premiseModel.PremiseTypeId));

            premiseType.PremiseIds.Add(premise.Id);

            await _premiseTypeRepository.UpdateAsync(premiseType);

            await CreateTags(premiseModel, premise.Id);

            return _mapper.Map<PremiseResponseModel>(premise);
        }

        public async Task<PremiseResponseModel> GetAsync(string id)
        {
            var premise = await _premiseRepository.FindOneByAsync(new PremiseById(id));

            var premiseType = await _premiseTypeRepository.FindOneByAsync(new PremiseTypeContainsPremise(id));

            var premiseModel = _mapper.Map<PremiseResponseModel>(premise);

            premiseModel.PremiseTypeName = premiseType.Name;          

            return premiseModel;
        }

        public async Task<PremiseResponseModel> UpdateAsync(PremiseModel premiseModel, UserContext userContext)
        {
            if (premiseModel.Id == null)
            {
                _logger.LogError($"Premise id is null!");
                throw new RecordNotFoundException($"Premise id is null!");
            }

            var premise = _mapper.Map<Premise>(premiseModel);

            await _authorizationHandler.Authorize(userContext, premise, Operation.UpdateOperation);

            var existingPremise = await _premiseRepository.GetAsync(premiseModel.Id);

            premise.Images = await _mediaService.UpdateImages(premiseModel.Images, existingPremise.Images);
            premise.MenuItems = await _mediaService.UpdateImages(premiseModel.MenuItems, existingPremise.MenuItems);

            await _locationService.AddOperatingCity(premiseModel.Location.City);

            await CreateTags(premiseModel, existingPremise.Id);

            await _premiseRepository.UpdateAsync(premise);

            foreach (var predefinedFilterId in premiseModel.PredefinedFilters)
            {
                var predefinedFilterDb = await _predefinedFilterRepository.GetAsync(predefinedFilterId);

                if (!predefinedFilterDb.PremiseIds.Any(x => x == premiseModel.Id))
                {
                    predefinedFilterDb.PremiseIds.Add(premise.Id);

                    _logger.LogInformation("Premise Id successfully added into predefined filters table.");

                    await _predefinedFilterRepository.UpdateAsync(predefinedFilterDb);
                }
            }

            var previousPremiseType = (await _premiseTypeRepository.FindManyByAsync(new PremiseTypeContainsPremise(premiseModel.Id))).First();
            previousPremiseType.PremiseIds.Remove(premiseModel.Id);

            await _premiseTypeRepository.UpdateAsync(previousPremiseType);

            var premiseType = await _premiseTypeRepository.FindOneByAsync(new PremiseTypeById(premiseModel.PremiseTypeId));
            premiseType.PremiseIds.Add(premiseModel.Id);

            await _premiseTypeRepository.UpdateAsync(premiseType);

            return _mapper.Map<PremiseResponseModel>(premise);
        }

        public async Task DeleteAsync(string id, UserContext userContext)
        {
            var premise = await _premiseRepository.FindOneByAsync(new PremiseById(id));

            var premiseTags = await _tagRepository.FindManyByAsync(new TagByPremiseId(id));

            foreach(var tag in premiseTags)
            {
                tag.PremiseIds.Remove(premise.Id);

                _logger.LogInformation("Tag is removed from deleted premise.");

                await _tagRepository.UpdateAsync(tag);
            }

            premise.IsDeleted = true;

            var premiseType = await _premiseTypeRepository.FindOneByAsync(new PremiseTypeContainsPremise(premise.Id));

            premiseType.PremiseIds.Remove(id);

            await _premiseTypeRepository.UpdateAsync(premiseType);

            await _authorizationHandler.Authorize(userContext, premise, Operation.DeleteOperation);

            await _premiseRepository.UpdateAsync(premise);
        }

        public async Task DeleteMenuItemAsync(string image, UserContext userContext)
        {
            var premise = await _premiseRepository.FindOneByAsync(new PremiseByMenuItemImage(image));

            await _authorizationHandler.Authorize(userContext, premise, Operation.CreateOperation);

            var menuItemToRemove = premise.MenuItems.Where(x => x.Path.Contains(image)).FirstOrDefault();

            if (menuItemToRemove == null)
            {
                throw new RecordNotFoundException("Menu item image not found.");
            }

            await _mediaService.DeleteImage(menuItemToRemove.Path);
            premise.MenuItems.Remove(menuItemToRemove);

            _logger.LogInformation("Menu item is removed from premise.");

            await _premiseRepository.UpdateAsync(premise);
        }

        public async Task<PaginatedList<PremiseResponseModel>> PremiseSearch(PremiseSearchModel searchModel, Pagination pagination)
        {
            var premiseSearch = await _premiseRepository.PremiseSearch(searchModel, pagination);

            return _mapper.Map<PaginatedList<PremiseResponseModel>>(premiseSearch);
        }

        public async Task<PaginatedList<PremiseTypeModel>> GetPremiseTypes()
        {
            var premiseTypes = await _premiseTypeRepository.GetAllAsync(new());
            var result = _mapper.Map<PaginatedList<PremiseTypeModel>>(premiseTypes);
            return result;
        }

        public async Task<IEnumerable<TagModel>> GetPremiseTags()
        {
            var premiseTags = await _tagRepository.FindManyByAsync(new ActiveTag());
            var result = _mapper.Map<IEnumerable<TagModel>>(premiseTags);
            return result;
        }

        #region Private methods
        private async Task CreateTags(PremiseModel premiseModel, string premiseId)
        {
            foreach (var tag in premiseModel.Tags.ConvertAll(d => d.ToLower()))
            {
                var tagModel = await _tagRepository.FindOneByAsync(new TagByName(tag));

                if (tagModel == null)
                {
                    Tag createTag = new Tag();

                    createTag.Name = tag;
                    createTag.PremiseIds.Add(premiseId);

                    _logger.LogInformation("Tag is created and added to premise.");

                    await _tagRepository.CreateAsync(createTag);
                }
                else
                {
                    if (!tagModel.PremiseIds.Contains(premiseId))
                    {
                        tagModel.PremiseIds.Add(premiseId);

                        _logger.LogInformation("Existing tag is added to premise.");

                        await _tagRepository.UpdateAsync(tagModel);
                    }
                }
            }
        }
        #endregion
    }
}
