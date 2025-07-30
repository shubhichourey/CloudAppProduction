using Azure.Communication.Email;
using Azure;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EmailProcessor;
using System;

public class EmailSenderService
{
    private readonly EmailClient _emailClient;
    private readonly ILogger<EmailSenderService> _logger;

    public EmailSenderService(string acsConnectionString, ILogger<EmailSenderService> logger)
    {
        _emailClient = new EmailClient(acsConnectionString);
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailDto email)
    {
        try
        {
            var emailMessage = new EmailMessage(
                senderAddress: "your-verified@domain.com", // Replace with your actual verified email
                recipients: new EmailRecipients(new[]
                {
                    new EmailAddress(email.To)
                }),
                content: new EmailContent(email.Subject ?? "No Subject")
                {
                    PlainText = email.Body,
                    Html = $"<html><body>{email.Body}</body></html>"
                });

            EmailSendOperation operation = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);
            _logger.LogInformation("Email sent to {To}. MessageId: {MessageId}", email.To, operation.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", email.To);
        }
    }
}
