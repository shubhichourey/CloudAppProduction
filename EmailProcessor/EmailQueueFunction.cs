using Azure;
using Azure.Communication.Email;
using EmailProcessor;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

public class EmailQueueFunction
{
    private readonly EmailClient _emailClient;
    private readonly ILogger<EmailQueueFunction> _logger;

    public EmailQueueFunction(EmailClient emailClient, ILogger<EmailQueueFunction> logger)
    {
        _emailClient = emailClient;
        _logger = logger;
    }

    [Function("SendEmailFromQueue")]
    public async Task RunAsync([QueueTrigger("emailqueue", Connection = "AzureWebJobsStorage")] string queueMessage)
    {
        try
        {
            var emailDto = JsonSerializer.Deserialize<EmailDto>(queueMessage);

            if (emailDto == null || string.IsNullOrWhiteSpace(emailDto.To))
            {
                _logger.LogWarning("Invalid message received.");
                return;
            }

            var content = new EmailContent(emailDto.Subject ?? "No Subject")
            {
                PlainText = emailDto.Body,
                Html = $"<html><body>{emailDto.Body}</body></html>"
            };

            var emailMessage = new EmailMessage(
            senderAddress: Environment.GetEnvironmentVariable("ACS_SENDER_EMAIL"),
            recipients: new EmailRecipients(new[]
            {
                new EmailAddress(emailDto.To)
            }),
            content: content
        );

            EmailSendOperation response = await _emailClient.SendAsync(WaitUntil.Completed, emailMessage);
            _logger.LogInformation($"Email sent to {emailDto.To}. Message ID: {response.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via ACS.");
        }
    }
}


