namespace MyHostAPI.Models
{
    public class TagModel
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public List<string> PremiseIds { get; set; } = new List<string>();
    }
}
