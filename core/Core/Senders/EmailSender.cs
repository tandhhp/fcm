using SendGrid.Helpers.Mail;
using SendGrid;
using Waffle.Core.Interfaces.IService;
using Waffle.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Waffle.Core.Senders
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly ISettingService _appService;

        public EmailSender(ILogger<EmailSender> logger, ISettingService appService)
        {
            _appService = appService;
            _logger = logger;
        }

        public async Task<IdentityResult> SendEmailAsync(string toEmail, string subject, string message) => await Execute(subject, message, toEmail);

        public async Task<IdentityResult> Execute(string subject, string message, string toEmail)
        {
            var app = await _appService.GetAsync<ExternalAPI.SendGrids.SendGrid>(nameof(SendGrid));
            if (string.IsNullOrEmpty(app?.ApiKey))
            {
                _logger.LogError("Null SendGridKey");
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "error.sendGridConfigurationNotFound",
                    Description = "SendGrid configuration not found"
                });
            }
            var client = new SendGridClient(app.ApiKey);
            var from_email = new EmailAddress(app.From.Email, app.From.Name);
            var to_email = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from_email, to_email, subject, string.Empty, message);
            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email to {toEmail} queued successfully!", toEmail);
            }
            else
            {
                _logger.LogError("Failed to send email: {toEmail}", toEmail);
            }
            return IdentityResult.Failed(new IdentityError
            {
                Code = response.StatusCode.ToString(),
                Description = await response.Body.ReadAsStringAsync()
            });
        }
    }
}
