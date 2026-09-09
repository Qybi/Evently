using Evently.Shared.Application.Caching;
using Evently.Shared.Application.Clock;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Caching;
using Evently.Shared.Infrastructure.Clock;
using Evently.Shared.Infrastructure.Interceptors;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Evently.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<IRegistrationConfigurator>[] moduleConfigureConsumers, string redisConnectionString)
    {
        services.TryAddSingleton<PublishDomainEventsInterceptor>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        try
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.TryAddSingleton(connectionMultiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch
        {
            services.AddDistributedMemoryCache();
        }

        services.TryAddSingleton<ICacheService, CacheService>();

        services.TryAddSingleton<IEventBus, EventBus.EventBus>();

        services.AddMassTransit((configure) =>
        {
            foreach (Action<IRegistrationConfigurator> configureConsumer in moduleConfigureConsumers)
            {
                configureConsumer(configure);
            }

            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
