namespace MyHostAPI.Common.Configurations
{
    public class DatabaseSection
    {
        public const string Name = "Database";
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string? Users { get; set; }
    }
}

