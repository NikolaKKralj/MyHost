namespace MyHostAPI.Models
{
    public class ResetPasswordModel
    {
        public string EncryptedEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
