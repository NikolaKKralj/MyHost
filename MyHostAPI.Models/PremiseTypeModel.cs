namespace MyHostAPI.Models
{
    public class PremiseTypeModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<string> PremiseIds { get; set; } = new List<string>();
        public string CoverImage { get; set; } = null!;
        public string Icon { get; set; } = null!;
    }
}
