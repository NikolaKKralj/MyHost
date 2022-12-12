using MyHostAPI.Domain.Reporting;

namespace MyHostAPI.Reporting.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail<T>(T model) where T : BaseReporting;
    }
}
