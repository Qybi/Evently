using Evently.Shared.Infrastructure.Configuration;
using Evently.Shared.Infrastructure.EventBus;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Evently.Ticketing.Api.Extensions;

internal static class HealthChecksExtensions
{
    internal static IServiceCollection AddHealthChecksInternal(this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMqSettings rabbitMqSettings = new(configuration.GetConnectionStringOrThrow("Queue"));

        // rabbitmq connection used ONLY by the health check, MassTransit will create its own connection to RabbitMQ
        services.AddSingleton(_ => CreateRabbitMqConnectionOrThrow(rabbitMqSettings));

        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionStringOrThrow("Database"))
            .AddRedis(configuration.GetConnectionStringOrThrow("Cache"))
            .AddRabbitMQ()
            .AddKeyCloak(configuration.GetKeyCloakHealthUrl());

        return services;
    }

    private static IConnection CreateRabbitMqConnectionOrThrow(RabbitMqSettings rabbitMqSettings)
    {
        ConnectionFactory factory = new()
        {
            Uri = new Uri(rabbitMqSettings.Host),
            UserName = rabbitMqSettings.Username,
            Password = rabbitMqSettings.Password
        };

        try
        {
            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }
        catch (BrokerUnreachableException ex)
        {
            throw new InvalidOperationException($"Could not connect to RabbitMQ at {factory.HostName}:{factory.Port}", ex);
        }
    }
}
