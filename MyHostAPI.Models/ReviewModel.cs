namespace MyHostAPI.Models
{
    public class ReviewModel 
    {
        public string? Id { get; set; } 
        public string PremiseId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
