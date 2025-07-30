using Azure.Storage.Queues;
using System.Text.Json;
using CloudApp.Application.DTOs;

public class QueueEmailService
{
    private readonly QueueClient _queueClient;

    public QueueEmailService(string connectionString, string queueName)
    {
        _queueClient = new QueueClient(connectionString, queueName);
        _queueClient.CreateIfNotExists();
    }

    public async Task QueueEmailAsync(EmailDto email)
    {
        var message = JsonSerializer.Serialize(email);
        await _queueClient.SendMessageAsync(message);
    }
}
