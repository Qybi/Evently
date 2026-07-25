using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Presentation.Events;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        EventEndpoints.MapEndpoints(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Application.AssemblyReference.Assembly);
        });

        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddDbContext<EventsDbContext>(
            options => options.UseNpgsql(
                databaseConnectionString,
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events)
            )
            .UseSnakeCaseNamingConvention()
        );

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddSingleton<IEventQueries, EventQueries>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
    }
}
