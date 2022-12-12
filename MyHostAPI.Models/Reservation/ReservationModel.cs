using MyHostAPI.Domain;

namespace MyHostAPI.Models
{
    public class ReservationModel
    {
        public string? Id { get; set; }
        public string PremiseId { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? ManagerId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Status Status { get; set; }
        public string? Note { get; set; }
    }
}
