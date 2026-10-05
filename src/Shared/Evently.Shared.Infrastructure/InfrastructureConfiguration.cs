using Evently.Shared.Application.Caching;
using Evently.Shared.Application.Clock;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Authentication;
using Evently.Shared.Infrastructure.Authorization;
using Evently.Shared.Infrastructure.Caching;
using Evently.Shared.Infrastructure.Clock;
using Evently.Shared.Infrastructure.Outbox;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
using StackExchange.Redis;

namespace Evently.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string serviceName, Action<IRegistrationConfigurator>[] moduleConfigureConsumers, string redisConnectionString)
    {
        services.AddAuthenticationInternal();

        services.AddAuthorizationInternal();

        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();

        // setting an Id for each instance, is useful for parallel testing when multiple quartz instances can spin up at the same time. The ids helps with recognizing each instance with
        // their jobs
        services.AddQuartz(configurator =>
        {
            var scheduler = Guid.NewGuid();
            configurator.ConfigureScheduler(options =>
            {
                options.InstanceId = $"default-id-{scheduler}";
                options.InstanceName = $"default-name-{scheduler}";
            });
        });

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

        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddNpgsql()
                    .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);

                tracing.AddOtlpExporter();
            });

        return services;
    }
}
