namespace MyHostAPI.Domain.Reporting
{
    public class ConfirmationEmail : BaseReporting
    {
        public override string TemplateName { get; protected set; } = "ConfirmationEmail.html";
        public string Subject { get; } = "Confirm your email address - myHost";
        public string ActivationLink { get; set; } = null!;
    }
}
