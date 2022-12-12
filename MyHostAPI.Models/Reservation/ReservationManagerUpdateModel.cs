using MyHostAPI.Domain;

namespace MyHostAPI.Models
{
    public class ReservationManagerUpdateModel
    {
        public string? Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Status Status { get; set; }
        public string? Note { get; set; }
    }
}
