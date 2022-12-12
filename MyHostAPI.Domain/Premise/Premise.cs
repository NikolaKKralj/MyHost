namespace MyHostAPI.Domain.Premise
{
    public class Premise : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<string> PhoneNumbers { get; set; } = null!;
        public List<Image> Images { get; set; } = null!;
        public string ManagerId { get; set; } = null!;
        public Location Location { get; set; } = null!;
        public List<Image> MenuItems { get; set; } = null!;
        public double RatingAverage { get; set; } = 0;
        public PremiseTimeSettings TimeSettings { get; set; } = null!;
    }
}