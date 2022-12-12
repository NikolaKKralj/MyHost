namespace MyHostAPI.Domain.Reporting
{
    public class ResetPasswordEmail : BaseReporting
    {
        public override string TemplateName { get; protected set; } = "ResetPasswordEmail.html";
        public string Subject { get; } = "Password Reset - myHost";
        public string ActivationLink { get; set; } = null!;
    }
}
