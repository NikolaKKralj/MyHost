namespace MyHostAPI.Domain.PredefinedFilter
{
    public class PredefinedFilter : BaseEntity
    {
        public string Name { get; set; } = null!;
        public List<string> PremiseIds { get; set; } = new List<string>();
    }
}
