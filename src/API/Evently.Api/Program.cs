using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Modules.Events.Infrastructure;
using Evently.Shared.Application;
using Evently.Shared.Infrastructure;
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
            Title = "Evently API",
            Version = "v1",
            Description = "API for Evently, a modular monolith event management platform."
        };

        return Task.CompletedTask;
    });
});

builder.Services.AddApplication([Evently.Modules.Events.Application.AssemblyReference.Assembly]);

builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Cache")!);

builder.Configuration.AddModuleConfiguration(["events"]);

builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}

EventsModule.MapEndpoints(app);

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.Run();
