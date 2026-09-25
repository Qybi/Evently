using Evently.Shared.Application.Caching;
using Evently.Shared.Application.Clock;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Authentication;
using Evently.Shared.Infrastructure.Authorization;
using Evently.Shared.Infrastructure.Caching;
using Evently.Shared.Infrastructure.Clock;
using Evently.Shared.Infrastructure.Interceptors;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using StackExchange.Redis;

namespace Evently.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<IRegistrationConfigurator>[] moduleConfigureConsumers, string redisConnectionString)
    {
        services.AddAuthenticationInternal();

        services.AddAuthorizationInternal();

        services.TryAddSingleton<PublishDomainEventsInterceptor>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

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

            // Include the namespace so same-named consumers in different modules get separate endpoints
            configure.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(includeNamespace: true));

            configure.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
