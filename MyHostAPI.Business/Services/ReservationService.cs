using AutoMapper;
using Microsoft.Extensions.Logging;
using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Exceptions;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Common.Models;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Data.Specifications;
using MyHostAPI.Domain;
using MyHostAPI.Models;
using static MyHostAPI.Data.Specifications.PremiseSpecification;
using static MyHostAPI.Data.Specifications.ReservationSpecification;

namespace MyHostAPI.Business.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IMapper _mapper;
        private readonly IPremiseRepository _premiseRepository;
        private readonly IAuthorizationHandler<Reservation> _authorizationHandler;
        private readonly ILogger<ReservationService> _logger;
        private readonly IUserRepository _userRepository;

        public ReservationService(IReservationRepository reservationRepository,
            IMapper mapper,
            IPremiseRepository premiseRepository,
            IAuthorizationHandler<Reservation> authorizationHandler,
            ILogger<ReservationService> loger,
            IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _premiseRepository = premiseRepository;
            _authorizationHandler = authorizationHandler;
            _logger = loger;
            _userRepository = userRepository;
        }
        public async Task CreateReservation(ReservationModel reservationModel, UserContext userContext)
        {

            reservationModel.Status = userContext.Role == Role.Customer ? Status.Requested : Status.Approved;

            var premise = await _premiseRepository.FindOneByAsync(new PremiseById(reservationModel.PremiseId));

            reservationModel.CustomerId = userContext.UserId;
            reservationModel.ManagerId = premise.ManagerId;

            var reservation = _mapper.Map<Reservation>(reservationModel);

            await _authorizationHandler.Authorize(userContext, reservation, Operation.CreateOperation);

            await _reservationRepository.CreateAsync(reservation);
        }


        public async Task DeleteReservation(string id, UserContext userContext)
        {
            var reservation = await _reservationRepository.FindOneByAsync(new ReservationById(id));

            reservation.IsDeleted = true;

            await _authorizationHandler.Authorize(userContext, reservation, Operation.DeleteOperation);

            await _reservationRepository.UpdateAsync(reservation);
        }

        public async Task<PaginatedList<ReservationModel>> GetAllReservations(Pagination pagination, UserContext userContext)
        {
            var reservations = await _reservationRepository.FindManyByAsync(new ActiveReservation(), pagination);

            reservations.ForEach(async x => await _authorizationHandler.Authorize(userContext, x, Operation.ReadOperation));

            var reservationModel = _mapper.Map<PaginatedList<ReservationModel>>(reservations);

            return reservationModel;
        }

        public async Task<PaginatedList<ReservationModel>> GetUserReservations(string id, Pagination pagination, UserContext userContext)
        {
            if (id != userContext.UserId)
            {
                _logger.LogError($"Input id doesn't match with user id");
                throw new UnauthorizedException();
            }

            var user = await _userRepository.FindOneByAsync(new UserById(id));

            if (user.Identity.Role == Role.Customer)
            {
                var reservations = await _reservationRepository.FindManyByAsync(new ActiveReservationByCustomer(user.Id), pagination);

                reservations.ForEach(async x => await _authorizationHandler.Authorize(userContext, x, Operation.ReadOperation));

                var reservationModel = _mapper.Map<PaginatedList<ReservationModel>>(reservations);

                return reservationModel;
            }
            else
            {
                var reservations = await _reservationRepository.FindManyByAsync(new ActiveReservationByManager(user.Id), pagination);

                reservations.ForEach(async x => await _authorizationHandler.Authorize(userContext, x, Operation.ReadOperation));

                var reservationModel = _mapper.Map<PaginatedList<ReservationModel>>(reservations);

                return reservationModel;
            }

        }

        public async Task<ReservationModel> GetReservationById(string id, UserContext userContext)
        {
            var reservation = await _reservationRepository.FindOneByAsync(new ReservationById(id));

            await _authorizationHandler.Authorize(userContext, reservation, Operation.ReadOperation);

            var reservationModel = _mapper.Map<ReservationModel>(reservation);

            return reservationModel;
        }

        public async Task ManagerUpdateReservation(ReservationManagerUpdateModel reservationModel, UserContext userContext)
        {
            if (reservationModel.Id == null)
            {
                _logger.LogError($"Reservation id is null!");
                throw new RecordNotFoundException($"Reservation id is null!");
            }

            var reservation = await _reservationRepository.FindOneByAsync(new ReservationById(reservationModel.Id));

            reservation.Start = reservationModel.Start;
            reservation.End = reservationModel.End;
            reservation.Status = reservationModel.Status;
            reservation.Note = reservationModel.Note;


            await _authorizationHandler.Authorize(userContext, reservation, Operation.UpdateOperation);

            await _reservationRepository.UpdateAsync(reservation);
        }

        public async Task CustomerUpdateReservation(ReservationCustomerUpdateModel reservationModel, UserContext userContext)
        {
            if (reservationModel.Id == null)
            {
                _logger.LogError($"Reservation id is null");
                throw new RecordNotFoundException($"Reservation id is null");
            }

            var reservation = await _reservationRepository.FindOneByAsync(new ReservationById(reservationModel.Id));

            reservation.Start = reservationModel.Start;
            reservation.End = reservationModel.End;
            reservation.Note = reservationModel.Note;

            await _authorizationHandler.Authorize(userContext, reservation, Operation.UpdateOperation);

            await _reservationRepository.UpdateAsync(reservation);
        }
    }
}
