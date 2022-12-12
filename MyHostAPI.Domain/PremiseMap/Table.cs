namespace MyHostAPI.Domain.PremiseMap
{
    public class Table : BaseEntity
    {
        public string TableTypeId { get; set; } = null!;
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
