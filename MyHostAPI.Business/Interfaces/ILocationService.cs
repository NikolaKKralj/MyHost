using MyHostAPI.Common.Helpers;
using MyHostAPI.Models;

namespace MyHostAPI.Business.Interfaces
{
    public interface ILocationService
    {
        Task AddOperatingCity(string city);
        Task<PaginatedList<OperatingCityModel>> GetOperatingCities();
        Task<OperatingCityModel> GetOperatingCityByUserLocation(LocationModel locationModel);
    }
}
