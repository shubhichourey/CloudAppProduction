using CloudApp.Application.Interfaces;
using CloudApp.Infrastructure.Data;
using CloudApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationInsightsTelemetry();

var keyVaultUrl = new Uri("https://cloudapp-keyvault.vault.azure.net/");

var secretClient = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());

KeyVaultSecret sendGridSecret = secretClient.GetSecret("SendGridApiKey");

builder.Configuration["SendGrid:ApiKey"] = sendGridSecret.Value;

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