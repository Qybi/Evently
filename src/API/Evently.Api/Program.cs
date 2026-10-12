using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Api.OpenTelemetry;
using Evently.Modules.Attendance.Infrastructure;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using Evently.Shared.Application;
using Evently.Shared.Infrastructure;
using Evently.Shared.Infrastructure.Configuration;
using Evently.Shared.Infrastructure.EventBus;
using Evently.Shared.Presentation.Endpoints;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using Serilog;
using Wolverine;

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
            Title = "Evently API",
            Version = "v1",
            Description = "API for Evently, a modular monolith event management platform."
        };

        return Task.CompletedTask;
    });
});

builder.Services.AddApplication([
    Evently.Modules.Events.Application.AssemblyReference.Assembly,
    Evently.Modules.Users.Application.AssemblyReference.Assembly,
    Evently.Modules.Attendance.Application.AssemblyReference.Assembly
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
        EventsModule.ConfigureWolverine,
        AttendanceModule.ConfigureWolverine,
        UsersModule.ConfigureWolverine
    ],
    databaseConnectionString,
    rabbitMqSettings);

builder.Configuration.AddModuleConfiguration(["events", "users", "attendance"]);

builder.Services.AddHealthChecksInternal(builder.Configuration);

builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddAttendanceModule(builder.Configuration);

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
