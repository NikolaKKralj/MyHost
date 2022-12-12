namespace MyHostAPI.Models
{
    public class LocationModel
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Number { get; set; }
        public string? Zip { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
