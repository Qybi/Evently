using Evently.Shared.Application.Caching;
using Evently.Shared.Application.Clock;
using Evently.Shared.Infrastructure.Caching;
using Evently.Shared.Infrastructure.Clock;
using Evently.Shared.Infrastructure.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Evently.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string redisConnectionString)
    {
        services.TryAddSingleton<PublishDomainEventsInterceptor>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.TryAddSingleton(connectionMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
        });

        services.TryAddSingleton<ICacheService, CacheService>();

        return services;
    }
}
