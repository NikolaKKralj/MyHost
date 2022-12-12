namespace MyHostAPI.Common.Configurations
{
    public class JwtSection
    {
        public const string Name = "JwtSettings";
        public string JWTKey { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
    }
}
