using CloudApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CloudApp.Infrastructure.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(IConfiguration config, ILogger<SendGridEmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (!MailAddress.TryCreate(toEmail, out _))
            {
                _logger.LogWarning("Invalid email format: {Email}", toEmail);
                return;
            }

            try
            {
                var apiKey = _config["SendGrid:ApiKey"];
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    _logger.LogError("SendGrid API key is not configured.");
                    return;
                }

                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("shubhi_chourey@epam.com", "CloudApp");
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, body, $"<strong>{body}</strong>");

                var response = await client.SendEmailAsync(msg);

                if ((int)response.StatusCode >= 400)
                {
                    var errorContent = await response.Body.ReadAsStringAsync();
                    _logger.LogWarning("SendGrid failed for {Email}. Status: {Status}, Body: {Body}",
                        toEmail, response.StatusCode, errorContent);
                }
                else
                {
                    _logger.LogInformation("Email sent to {Email} successfully.", toEmail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
            }
        }
    }
}
