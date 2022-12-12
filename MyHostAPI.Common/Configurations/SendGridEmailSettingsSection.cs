namespace MyHostAPI.Common.Configurations
{
    public class SendGridEmailSettingsSection
    {
        public const string Name = "SendGridEmailSettings";
        public string APIKey { get; set; } = null!;
        public string ConfirmationLink { get; set; } = null!;
        public string ResetPasswordLink { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
        public string FromName { get; set; } = null!;
        public string FromAddress { get; set; } = null!;
        public string FromCity { get; set; } = null!;
        public string FromState { get; set; } = null!;
        public string FromZip { get; set; } = null!;
        public string RelativePath { get; set; } = null!;
    }
}
