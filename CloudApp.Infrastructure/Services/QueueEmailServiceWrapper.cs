using CloudApp.Application.DTOs;
using CloudApp.Application.Interfaces;

namespace CloudApp.Infrastructure.Services
{
    public class QueueEmailServiceWrapper : IEmailService
    {
        private readonly QueueEmailService _queueService;

        public QueueEmailServiceWrapper(QueueEmailService queueService)
        {
            _queueService = queueService;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string bodyHtml)
        {
            var email = new EmailDto
            {
                To = toEmail,
                Subject = subject,
                Body = bodyHtml
            };

            await _queueService.QueueEmailAsync(email);
        }
    }
}
