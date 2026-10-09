using Evently.Modules.Ticketing.Infrastructure;
using Evently.Shared.Application;
using Evently.Shared.Infrastructure;
using Evently.Shared.Infrastructure.Configuration;
using Evently.Shared.Infrastructure.EventBus;
using Evently.Shared.Presentation.Endpoints;
using Evently.Ticketing.Api.Extensions;
using Evently.Ticketing.Api.Middleware;
using Evently.Ticketing.Api.OpenTelemetry;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "Evently Ticketing API",
            Version = "v1",
            Description = "API for Evently Ticketing, a service for managing event tickets."
        };

        return Task.CompletedTask;
    });
});

builder.Services.AddApplication([
    Evently.Modules.Ticketing.Application.AssemblyReference.Assembly
]);

string databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");
string cacheConnectionString = builder.Configuration.GetConnectionStringOrThrow("Cache");
RabbitMqSettings rabbitMqSettings = new(builder.Configuration.GetConnectionStringOrThrow("Queue"));

builder.Services.AddInfrastructure(
    DiagnosticsConfig.ServiceName,
    cacheConnectionString);

builder.Services.AddWolverineInternal(
    DiagnosticsConfig.ServiceName,
    [
        TicketingModule.ConfigureWolverine
    ],
    databaseConnectionString,
    rabbitMqSettings);

builder.Configuration.AddModuleConfiguration(["ticketing"]);

builder.Services.AddHealthChecksInternal(builder.Configuration);

builder.Services.AddTicketingModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}

app.MapEndpoints();

app.MapHealthChecks("health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseLogContextTraceLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

await app.RunAsync();
