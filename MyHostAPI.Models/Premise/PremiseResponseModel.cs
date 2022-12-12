using MyHostAPI.Domain.Premise;

namespace MyHostAPI.Models.Premise
{
    public class PremiseResponseModel
    {
        public string? Id { get; set; }
        public List<ImageModel> Images { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<string> PhoneNumbers { get; set; } = null!;
        public string ManagerId { get; set; } = null!;
        public LocationModel Location { get; set; } = null!;
        public List<ImageModel> MenuItems { get; set; } = null!;
        public double RatingAverage { get; set; }
        public List<string> Tags { get; set; } = new();
        public string PremiseTypeName { get; set; } = null!;
        public PremiseTimeSettingsModel TimeSettings { get; set; } = null!;
    }
}

