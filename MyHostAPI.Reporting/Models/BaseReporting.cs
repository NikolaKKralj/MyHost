namespace MyHostAPI.Domain.Reporting
{
    public abstract class BaseReporting
    {
        public string ToEmail { get; set; } = null!;
        public abstract string TemplateName { get; protected set; }

    }
}
