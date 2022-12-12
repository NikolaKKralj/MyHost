namespace MyHostAPI.Domain
{
    public class Review : BaseEntity
    {
        public string PremiseId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
    }
}

