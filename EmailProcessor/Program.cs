//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.Functions.Worker.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;

//var builder = FunctionsApplication.CreateBuilder(args);

//builder.ConfigureFunctionsWebApplication();

//builder.Services
//    .AddApplicationInsightsTelemetryWorkerService()
//    .ConfigureFunctionsApplicationInsights();

//builder.Build().Run();


using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Azure.Communication.Email;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication() 
    .ConfigureServices((context, services) =>
    {
        // Register Application Insights
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Register Azure EmailClient using connection string from configuration
        var configuration = context.Configuration;
        var emailConnectionString = configuration["ACS_CONNECTION_STRING"];
        services.AddSingleton(new EmailClient(emailConnectionString));
    })
    .Build();

host.Run();


