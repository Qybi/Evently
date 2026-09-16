using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Categories;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Application.TicketTypes;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Queries;
using Evently.Modules.Events.Infrastructure.Repositories;
using Evently.Shared.Infrastructure.Interceptors;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")!;

        services.AddDbContext<EventsDbContext>(
            (sp, options) => options
            .UseNpgsql(
                databaseConnectionString,
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events)
            )
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>())
        );

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();

        services.AddScoped<IEventQueries, EventQueries>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<ITicketTypeQueries, TicketTypeQueries>();
    }
}
