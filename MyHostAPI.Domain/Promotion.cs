namespace MyHostAPI.Domain
{
    public class Promotion : BaseEntity
    {
        public string PremiseId { get; set; } = null!;
        public string EventId { get; set; } = null!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}

