namespace MyHostAPI.Models
{
    public class ReservationCustomerUpdateModel
    {
        public string? Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Note { get; set; }
    }
}
