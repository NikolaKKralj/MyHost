namespace MyHostAPI.Domain.PremiseMap
{
    public class Map : BaseEntity
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public string PremiseId { get; set; } = null!;
        public List<Table> Tables { get; set; } = new List<Table>();
    }
}
