using Azure.Communication.Email;
using CloudApp.Application.Interfaces; 
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Azure;

namespace CloudApp.Infrastructure.Services
{
    public class AzureEmailService : IEmailService
    {
        private readonly EmailClient _emailClient;
        private readonly string _senderAddress;

        public AzureEmailService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureEmail:ConnectionString"];
            _senderAddress = configuration["AzureEmail:SenderAddress"];

            _emailClient = new EmailClient(connectionString);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string bodyHtml)
        {
            var emailContent = new EmailContent(subject)
            {
                Html = bodyHtml
            };

            var emailRecipients = new EmailRecipients(new[] { new EmailAddress(toEmail) });

            var message = new EmailMessage(
                senderAddress:_senderAddress,
                content: emailContent,
                recipients: emailRecipients
            );

            EmailSendOperation emailSendOperation = await _emailClient.SendAsync(WaitUntil.Completed, message);
            Console.WriteLine($"Email sent. Status: {emailSendOperation.Value.Status}");
        }
    }
}
