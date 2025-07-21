using Azure.Storage.Queues;
using CloudApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloudApp.Infrastructure.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SendGridEmailService> _logger;
        private readonly QueueClient _queueClient;

        public SendGridEmailService(IConfiguration config, ILogger<SendGridEmailService> logger, QueueClient queueClient)
        {
            _config = config;
            _logger = logger;
            _queueClient = queueClient;
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
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("shubhi_chourey@epam.com", "CloudApp");
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, body, $"<strong>{body}</strong>");

                var response = await client.SendEmailAsync(msg);

                if ((int)response.StatusCode >= 400)
                {
                    _logger.LogWarning("SendGrid failed for {Email}. Queuing instead. Status: {Status}", toEmail, response.StatusCode);
                    await QueueEmailAsync(toEmail, subject, body);
                }
                else
                {
                    _logger.LogInformation("Email sent to {Email} successfully.", toEmail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendGrid error. Queuing email for {Email}", toEmail);
                await QueueEmailAsync(toEmail, subject, body);
            }
        }

        private async Task QueueEmailAsync(string to, string subject, string body)
        {
            var message = JsonSerializer.Serialize(new { To = to, Subject = subject, Body = body });
            await _queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message)));
        }
    }

}
