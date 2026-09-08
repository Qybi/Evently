using Evently.Modules.Ticketing.Application.Carts;
using Evently.Shared.Presentation.Endpoints;
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
#pragma warning disable S1172
#pragma warning disable IDE0060
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
#pragma warning restore S1172
#pragma warning restore IDE0060
    {
        services.AddSingleton<CartService>();
    }
}
