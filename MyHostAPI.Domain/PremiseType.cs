namespace MyHostAPI.Domain.Premise
{
    public class PremiseType : BaseEntity
    {
        public string Name { get; set; } = null!;
        public List<string> PremiseIds { get; set; } = new List<string>();
        public string CoverImage { get; set; } = null!;
        public string Icon { get; set; } = null!;
    }
}
