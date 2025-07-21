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
                var emailData = new
                {
                    To = toEmail,
                    Subject = subject,
                    Body = body
                };

                string message = JsonSerializer.Serialize(emailData);

                await _queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message)));

                _logger.LogInformation("Email message queued for {Email}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to queue email to {Email}", toEmail);
            }
        }
    }
}
