namespace MyHostAPI.Common.Configurations
{
    public class EncryptionSection
    {
        public const string Name = "EncryptionSettings";
        public string EncryptionKey { get; set; } = string.Empty;
    }
}
