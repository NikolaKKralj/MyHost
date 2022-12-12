namespace MyHostAPI.Domain.PremiseMap
{
    public class TableType : BaseEntity
    {
        public string Name { get; set; } = null!;
        public double Width { get; set; }
        public double Height { get; set; }
        public int Capacity { get; set; }
    }
}
