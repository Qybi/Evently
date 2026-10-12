using JasperFx.Resources;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.Postgresql;
using Wolverine.RabbitMQ;
using Wolverine.Transports;

namespace Evently.Shared.Infrastructure.EventBus;

public static class WolverineExtensions
{
    public static IServiceCollection AddWolverineInternal(this IServiceCollection services,
        string serviceName,
        Action<WolverineOptions>[] moduleConfigureWolverine,
        string databaseConnectionString,
        RabbitMqSettings rabbitMqSettings)
    {
        services.AddWolverine(options =>
        {
            options.ServiceName = serviceName;

            // Durable inbox/outbox in Postgres. One schema per service because all APIs share the same database
            string schemaName = $"wolverine_{serviceName.ToLowerInvariant().Replace('.', '_')}"; // Evently.Api -> wolverine_evently_api
            options.PersistMessagesWithPostgresql(databaseConnectionString, schemaName);

            options
                .UseRabbitMq(factory =>
                {
                    factory.Uri = new Uri(rabbitMqSettings.Host); // host, port and vhost from the amqp:// connection string
                    factory.UserName = rabbitMqSettings.Username;
                    factory.Password = rabbitMqSettings.Password;
                })
                .AutoProvision() // create missing exchanges, queues and bindings on the broker
                .UseConventionalRouting(NamingSource.FromHandlerType); // one exchange per message type, one queue per handler type

            // Send through RabbitMQ even when the handler lives in this same process
            options.Policies.DisableConventionalLocalRouting();

            // Many modules can handle the same message: each handler runs on its own, with its own retries and failures
            options.MultipleHandlerBehavior = MultipleHandlerBehavior.Separated;

            // Inbox deduplicates by message id + receiving queue, so one message delivered to many handler queues is not a duplicate
            options.Durability.MessageIdentity = MessageIdentity.IdAndDestination;

            // Single node: no leader election, persisted envelopes recovered right away on restart. Use Balanced with multiple instances
            options.Durability.Mode = DurabilityMode.Solo;

            options.UseRuntimeCompilation(); // enable runtime compilation of message handlers, instead of manually running `dotnet run -- codegene write` after each change

            // Each module registers its own handlers and sagas
            foreach (Action<WolverineOptions> configureWolverine in moduleConfigureWolverine)
            {
                configureWolverine(options);
            }
        });

        // Set up every stateful resource (Postgres envelope tables, RabbitMQ objects) on startup
        services.AddResourceSetupOnStartup();

        return services;
    }
}
