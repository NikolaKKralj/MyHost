using AutoMapper;
using GeoCoordinatePortable;
using Microsoft.Extensions.Logging;
using MyHostAPI.Business.Interfaces;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Data.Interfaces;
using MyHostAPI.Models;
using static MyHostAPI.Data.Specifications.OperatingCitySpecification;

namespace MyHostAPI.Business.Services
{
    public class LocationService : ILocationService
    {
        private readonly IOperatingCityRepository _operatingCityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationService> _logger;

        public LocationService(IOperatingCityRepository operatingCityRepository, IMapper mapper, ILogger<LocationService> logger)
        {
            _operatingCityRepository = operatingCityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddOperatingCity(string city)
        {
            var operatingCity = await _operatingCityRepository.FindOneByAsync(new OperatingCityByName(city));

            if (operatingCity == null)
            {
                await _operatingCityRepository.CreateAsync(new Domain.OperatingCity() { Name = city });

                _logger.LogInformation("Successfully added operating city.");
            }
        }

        public async Task<PaginatedList<OperatingCityModel>> GetOperatingCities()
        {
            var operatingCities = await _operatingCityRepository.FindManyByAsync(new ActiveOperatingCities());

            return _mapper.Map<PaginatedList<OperatingCityModel>>(operatingCities);
        }

        public async Task<OperatingCityModel> GetOperatingCityByUserLocation(LocationModel locationModel)
        {
            var userCoordinates = new GeoCoordinate(locationModel.Lat, locationModel.Lng);

            var closestCoordinates = (await _operatingCityRepository.FindManyByAsync(new ActiveOperatingCities())).Select(x => new GeoCoordinate(x.Lat, x.Lng))
                .OrderBy(x => x.GetDistanceTo(userCoordinates))
                .FirstOrDefault();

            var closestCity = await _operatingCityRepository.FindOneByAsync(new OperatingCityByCoordinates(closestCoordinates.Latitude, closestCoordinates.Longitude));

            return _mapper.Map<OperatingCityModel>(closestCity);
        }
    }
}
