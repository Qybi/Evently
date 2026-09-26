using Evently.Modules.Users.Infrastructure.Database;
using Evently.Shared.Application.Clock;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Inbox;
using Evently.Shared.Infrastructure.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Evently.Modules.Users.Infrastructure.Inbox;

[DisallowConcurrentExecution]
internal sealed class ProcessInboxJob(UsersDbContext usersDbContext,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<InboxOptions> inboxOptions,
    ILogger<ProcessInboxJob> logger) : IJob
{
    private const string ModuleName = "Users";
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("{Module} - Beginning to process inbox messages", ModuleName);

        await using IDbContextTransaction transaction = await usersDbContext.Database.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<InboxMessage> inboxMessages = await GetInboxMessagesAsync(cancellationToken);

        foreach (InboxMessage inboxMessage in inboxMessages)
        {
            Exception? exception = null;
            try
            {
                IIntegrationEvent integrationEvent = JsonConvert.DeserializeObject<IIntegrationEvent>(
                    inboxMessage.Content,
                    SerializerSettings.Instance)!;

                await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();

                IEnumerable<IIntegrationEventHandler> integrationEventHandlers = IntegrationEventHandlersFactory.GetHandlers(
                    integrationEvent.GetType(),
                    scope.ServiceProvider,
                    Presentation.AssemblyReference.Assembly);

                foreach (IIntegrationEventHandler integrationEventHandler in integrationEventHandlers)
                {
                    await integrationEventHandler.Handle(integrationEvent, cancellationToken);
                }
            }
            catch (Exception caughtException)
            {
                logger.LogError(caughtException, "{Module} - Exception while processing inbox message {MessageId}", ModuleName, inboxMessage.Id);

                exception = caughtException;
            }

            inboxMessage.ProcessedOnUtc = dateTimeProvider.UtcNow;
            inboxMessage.Error = exception?.ToString();
        }

        await usersDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation("{Module} - Completed processing inbox messages", ModuleName);
    }

    private async Task<IReadOnlyList<InboxMessage>> GetInboxMessagesAsync(CancellationToken cancellationToken)
    {
        // single instance - in case of multiple instances add SKIP LOCKED after FOR UPDATE
        List<InboxMessage> inboxMessages = await usersDbContext.Set<InboxMessage>()
            .FromSql($"""
                SELECT *
                FROM users.inbox_messages
                WHERE processed_on_utc IS NULL
                ORDER BY occurred_on_utc
                LIMIT {inboxOptions.Value.BatchSize}
                FOR UPDATE
            """)
            .ToListAsync(cancellationToken);

        return inboxMessages;
    }
}
