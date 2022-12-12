using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyHostAPI.Common.Configurations;
using MyHostAPI.Common.Exceptions;
using MyHostAPI.Domain.Reporting;
using MyHostAPI.Reporting.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyHostAPI.Reporting.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly SendGridEmailSettingsSection _sendGridEmailSettingsSection;
        private readonly IMapper _mapper;

        public EmailService(ISendGridClient sendGridClient,
            IConfiguration configuration,
            ILogger<EmailService> logger,
            IOptions<SendGridEmailSettingsSection> storageOptions,
            IMapper mapper)
        {
            _sendGridClient = sendGridClient;
            _configuration = configuration;
            _logger = logger;
            _sendGridEmailSettingsSection = storageOptions.Value;
            _mapper = mapper;
        }

        public async Task SendEmail<T>(T model) where T : BaseReporting
        {
            try
            {
                var commonInfo = _mapper.Map<MyHostInformation>(_sendGridEmailSettingsSection);
                var commonProps = typeof(MyHostInformation).GetProperties();
                var props = typeof(T).GetProperties();

                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = _sendGridEmailSettingsSection.RelativePath;
                string sFile = Path.GetFullPath(Path.Combine(currentDirectory, relativePath, model.TemplateName));
                string strHTML = File.ReadAllText(sFile);
                string subject = "";

                //common information
                foreach (var commonProp in commonProps)
                {
                    strHTML = strHTML.Replace($"{{{{{commonProp.Name}}}}}", (string?)commonProp.GetValue(commonInfo, null));
                }

                //specifically information
                foreach (var prop in props)
                {
                    strHTML = strHTML.Replace($"{{{{{prop.Name}}}}}", (string?)prop.GetValue(model, null));
                    if (prop.Name == "Subject")
                    {
                        subject = (string?)prop.GetValue(model, null) ?? "";
                    }
                }

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(commonInfo.FromEmail, commonInfo.FromName),
                    Subject = subject,
                    HtmlContent = strHTML

                };

                msg.AddTo(model.ToEmail);

                var response = await _sendGridClient.SendEmailAsync(msg);
                _logger.LogInformation("Email sending status:" + response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Email Sending Failed");
                throw new InvalidSendMailException($"Email sending failed :  {ex.Message}");
            }
        }
    }
}
