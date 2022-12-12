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
using static MyHostAPI.Data.Specifications.EventSpecification;
using static MyHostAPI.Data.Specifications.PremiseSpecification;

namespace MyHostAPI.Business.Services
{
    public class EventService : IEventService
    {
        private readonly IPremiseRepository _premiseRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationHandler<Premise> _authorizationHandlerPremise;
        private readonly IAuthorizationHandler<Event> _authorizationHandlerEvent;
        private readonly ILogger<EventService> _logger;

        public EventService(IEventRepository eventRepository,
            IPremiseRepository premiseRepository,
            IMapper mapper,
            IAuthorizationHandler<Premise> authorizationHandlerPremise,
            IAuthorizationHandler<Event> authorizationHandlerEvent,
            ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _premiseRepository = premiseRepository;
            _mapper = mapper;
            _authorizationHandlerPremise = authorizationHandlerPremise;
            _authorizationHandlerEvent = authorizationHandlerEvent;
            _logger = logger;
        }

         public async Task CreateEvent(EventModel eventModel, UserContext userContext)
        {
            var premise = await _premiseRepository.FindOneByAsync(new PremiseById(eventModel.PremiseId));

            await _authorizationHandlerPremise.Authorize(userContext, premise, Operation.UpdateOperation);

            var mappedEvent = _mapper.Map<Event>(eventModel);

            await _authorizationHandlerEvent.Authorize(userContext, mappedEvent, Operation.CreateOperation);

            await _eventRepository.CreateAsync(mappedEvent);
        }

        public async Task DeleteEvent(string id, UserContext userContext)
        {
            var selectedEvent = await _eventRepository.FindOneByAsync(new EventById(id));

            var premise = await _premiseRepository.FindOneByAsync(new PremiseById(selectedEvent.PremiseId));

            await _authorizationHandlerPremise.Authorize(userContext, premise, Operation.UpdateOperation);

            selectedEvent.IsDeleted = true;

            await _authorizationHandlerEvent.Authorize(userContext, selectedEvent, Operation.DeleteOperation);

            await _eventRepository.UpdateAsync(selectedEvent);
        
        }

        public async Task<PaginatedList<EventModel>> GetAll(Pagination pagination, UserContext userContext)
        {
            var events = await _eventRepository.FindManyByAsync(new ActiveEvent(), pagination);

            events.ForEach(async x => await _authorizationHandlerEvent.Authorize(userContext, x, Operation.ReadOperation));

            return _mapper.Map<PaginatedList<EventModel>>(events);
        }

        public async Task<EventModel> GetEventById(string id, UserContext userContext)
        {
            var selectedEvent = await _eventRepository.FindOneByAsync(new EventById(id));

            await _authorizationHandlerEvent.Authorize(userContext, selectedEvent, Operation.ReadOperation);

            return _mapper.Map<EventModel>(selectedEvent);
        }

        public async Task<PaginatedList<EventModel>> GetPremiseEvents(string premiseid, Pagination pagination, UserContext userContext)
        {
            var premiseEvents = await _eventRepository.FindManyByAsync(new EventsByPremiseId(premiseid), pagination);

            premiseEvents.ForEach(async x => await _authorizationHandlerEvent.Authorize(userContext, x, Operation.ReadOperation));

            return _mapper.Map<PaginatedList<EventModel>>(premiseEvents);
        }

        public async Task UpdateEvent(EventUpdateModel eventUpdateModel, UserContext userContext)
        {
            if (eventUpdateModel.Id == null)
            {
                _logger.LogError($"Event id is null!");
                throw new RecordNotFoundException($"Event id is null!");
            }

            var existingEvent = await _eventRepository.FindOneByAsync(new EventById(eventUpdateModel.Id));

            var premise = await _premiseRepository.FindOneByAsync(new PremiseById(existingEvent.PremiseId));

            await _authorizationHandlerPremise.Authorize(userContext, premise, Operation.UpdateOperation);

            var mappedEvent = _mapper.Map(eventUpdateModel, existingEvent);

            await _authorizationHandlerEvent.Authorize(userContext, mappedEvent, Operation.UpdateOperation);

            await _eventRepository.UpdateAsync(mappedEvent);
        }
    }
}
