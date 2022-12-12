namespace MyHostAPI.Models
{
    public class EventModel
    {
        public string? Id { get; set; }
        public string PremiseId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Description { get; set; }
    }
}
