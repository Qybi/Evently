using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Application.Customers;
using Evently.Modules.Ticketing.Infrastructure.Queries;
using Evently.Modules.Ticketing.Infrastructure.Repositories;
using Evently.Modules.Ticketing.Presentation.Customers;
using Evently.Shared.Presentation.Endpoints;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
    }

#pragma warning disable S1172
#pragma warning disable IDE0060
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
#pragma warning restore S1172
#pragma warning restore IDE0060
    {
        services.AddSingleton<CartService>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerQueries, CustomerQueries>();
    }
}
