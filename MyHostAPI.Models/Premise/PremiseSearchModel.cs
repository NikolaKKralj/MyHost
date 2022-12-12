using MyHostAPI.Domain.Premise;

namespace MyHostAPI.Models.Premise
{
    public class PremiseSearchModel
    {
        public string? SearchInput { get; set; }
        public PremiseSort SortBy { get; set; }
        public bool OrderByDescending { get; set; }
        public int Rating { get; set; }
        public string? PremiseTypeId { get; set; }
        public List<string> PredefinedFiltersId { get; set; } = new List<string>();
        public string? OperatingCity { get; set; }
    }
}
