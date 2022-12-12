using MyHostAPI.Domain.Premise;

namespace MyHostAPI.Models.Premise
{
    public class PremiseModel
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<string> PhoneNumbers { get; set; } = null!;
        public string ManagerId { get; set; } = null!;
        public LocationModel Location { get; set; } = null!;
        public List<ImageModel> MenuItems { get; set; } = null!;
        public List<ImageModel> Images { get; set; } = null!;
        public PremiseTimeSettingsModel TimeSettings { get; set; } = null!;
        public double RatingAverage { get; set; }
        public List<string> PredefinedFilters { get; set; } = new List<string>();
        public string PremiseTypeId { get; set; } = null!;
        public List<string> Tags { get; set; } = new();
    }
}
