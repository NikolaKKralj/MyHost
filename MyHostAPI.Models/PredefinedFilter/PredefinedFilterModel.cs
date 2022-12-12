namespace MyHostAPI.Models.PredefinedFilter
{
    public class PredefinedFilterModel
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public List<string> Premises { get; set; } = new List<string>();
    }
}
