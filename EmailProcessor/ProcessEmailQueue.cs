using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

public class EmailDto
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class ProcessEmailQueue
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;

    public ProcessEmailQueue(ILoggerFactory loggerFactory, IConfiguration config)
    {
        _logger = loggerFactory.CreateLogger<ProcessEmailQueue>();
        _config = config;
    }

    [Function("ProcessEmailQueue")]
    public async Task Run([QueueTrigger("emailqueue", Connection = "AzureWebJobsStorage")] string base64Message)
    {
        try
        {
            var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Message));
            var email = JsonSerializer.Deserialize<EmailDto>(json);

            var client = new SendGridClient(_config["SendGrid:ApiKey"]);
            var from = new EmailAddress("shubhi_chourey@epam.com", "CloudApp Retry");
            var to = new EmailAddress(email.To);
            var msg = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, $"<strong>{email.Body}</strong>");
            var response = await client.SendEmailAsync(msg);

            _logger.LogInformation($"✅ Retried email sent to {email.To} with status {response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Failed to send queued email: {ex.Message}");
        }
    }
}
