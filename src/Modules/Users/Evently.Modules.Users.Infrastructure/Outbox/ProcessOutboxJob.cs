using Evently.Modules.Users.Infrastructure.Database;
using Evently.Shared.Application.Clock;
using Evently.Shared.Domain.DomainEvents;
using Evently.Shared.Infrastructure.Outbox;
using Evently.Shared.Infrastructure.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Evently.Modules.Users.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(UsersDbContext usersDbContext,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxJob> logger) : IJob
{
    private const string ModuleName = "Users";
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("{Module} - Beginning to process outbox messages", ModuleName);

        await using IDbContextTransaction transaction = await usersDbContext.Database.BeginTransactionAsync(cancellationToken);

        IReadOnlyList<OutboxMessage> outboxMessages = await GetOutboxMessagesAsync(cancellationToken);

        foreach (OutboxMessage outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    SerializerSettings.Instance)!;

                await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();

                IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                await publisher.Publish(domainEvent, cancellationToken);
            }
            catch (Exception caughtException)
            {
                logger.LogError(caughtException, "{Module} - Exception while processing outbox message {MessageId}", ModuleName, outboxMessage.Id);

                exception = caughtException;
            }

            outboxMessage.ProcessedOnUtc = dateTimeProvider.UtcNow;
            outboxMessage.Error = exception?.ToString();
        }

        await usersDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation("{Module} - Completed processing outbox messages", ModuleName);
    }

    private async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        // single instance - in case of multiple instances add SKIP LOCKED after FOR UPDATE
        List<OutboxMessage> outboxMessages = await usersDbContext.Set<OutboxMessage>()
            .FromSql($"""
                SELECT *
                FROM users.outbox_messages
                WHERE processed_on_utc IS NULL
                ORDER BY occurred_on_utc
                LIMIT {outboxOptions.Value.BatchSize}
                FOR UPDATE
            """)
            .ToListAsync(cancellationToken);


        return outboxMessages;
    }
}
