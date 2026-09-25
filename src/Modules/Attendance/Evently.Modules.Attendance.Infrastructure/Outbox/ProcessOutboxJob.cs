using Evently.Modules.Attendance.Infrastructure.Database;
using Evently.Shared.Application.Clock;
using Evently.Shared.Application.Messaging;
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

namespace Evently.Modules.Attendance.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(AttendanceDbContext attendanceDbContext,
    IServiceScopeFactory serviceScopeFactory,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxJob> logger) : IJob
{
    private const string ModuleName = "Attendance";
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("{Module} - Beginning to process outbox messages", ModuleName);

        await using IDbContextTransaction transaction = await attendanceDbContext.Database.BeginTransactionAsync(cancellationToken);

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

                IEnumerable<IDomainEventHandler> domainEventHandlers = DomainEventHandlersFactory.GetHandlers(
                    domainEvent.GetType(),
                    scope.ServiceProvider,
                    Application.AssemblyReference.Assembly);

                foreach (IDomainEventHandler domainEventHandler in domainEventHandlers)
                {
                    await domainEventHandler.Handle(domainEvent, cancellationToken);
                }
            }
            catch (Exception caughtException)
            {
                logger.LogError(caughtException, "{Module} - Exception while processing outbox message {MessageId}", ModuleName, outboxMessage.Id);

                exception = caughtException;
            }

            outboxMessage.ProcessedOnUtc = dateTimeProvider.UtcNow;
            outboxMessage.Error = exception?.ToString();
        }

        await attendanceDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation("{Module} - Completed processing outbox messages", ModuleName);
    }

    private async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessagesAsync(CancellationToken cancellationToken)
    {
        // single instance - in case of multiple instances add SKIP LOCKED after FOR UPDATE
        List<OutboxMessage> outboxMessages = await attendanceDbContext.Set<OutboxMessage>()
            .FromSql($"""
                SELECT *
                FROM attendance.outbox_messages
                WHERE processed_on_utc IS NULL
                ORDER BY occurred_on_utc
                LIMIT {outboxOptions.Value.BatchSize}
                FOR UPDATE
            """)
            .ToListAsync(cancellationToken);

        return outboxMessages;
    }
}
