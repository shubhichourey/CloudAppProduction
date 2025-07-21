using CloudApp.Application.Interfaces;
using CloudApp.Infrastructure.Data;
using CloudApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Queues;

var builder = WebApplication.CreateBuilder(args);

// Enable Application Insights
builder.Services.AddApplicationInsightsTelemetry();

// Connect to Azure Key Vault
var keyVaultUrl = new Uri("https://cloudapp-keyvault.vault.azure.net/");
var secretClient = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());

// Get secrets
KeyVaultSecret sendGridSecret = secretClient.GetSecret("SendGridApiKey");
KeyVaultSecret queueConnectionSecret = secretClient.GetSecret("StorageQueueConnection");

// Correctly map secrets to configuration
builder.Configuration["SendGrid:ApiKey"] = sendGridSecret.Value;
builder.Configuration["ConnectionStrings:StorageQueueConnection"] = queueConnectionSecret.Value;
builder.Configuration["AzureStorageQueue:QueueName"] = "emailqueue"; // Your queue name

// Add services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://victorious-cliff-06d37fb00.2.azurestaticapps.net")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, SendGridEmailService>();

// Register Azure QueueClient using correct key name
builder.Services.AddSingleton(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("StorageQueueConnection");
    var queueName = builder.Configuration["AzureStorageQueue:QueueName"];
    var client = new QueueClient(connectionString, queueName);
    client.CreateIfNotExists();
    return client;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAngularClient");

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();
