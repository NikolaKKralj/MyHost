using MyHostAPI.Domain;

namespace MyHostAPI.Data.Specifications
{
    public class OperatingCitySpecification
    {
        public class OperatingCityByName : BaseSpecification<OperatingCity>
        {
            public OperatingCityByName(string name) : base(b => b.Name == name)
            {

            }
        }

        public class ActiveOperatingCities : BaseSpecification<OperatingCity>
        {
            public ActiveOperatingCities() : base(b => b.IsDeleted == false)
            {

            }
        }

        public class OperatingCityByCoordinates : BaseSpecification<OperatingCity>
        {
            public OperatingCityByCoordinates(double latitude, double longtitude) : base(b => b.Lng == longtitude && b.Lat == latitude)
            {

            }
        }
    }
}
