namespace MyHostAPI.Domain
{
    public class OperatingCity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
