using Evently.Modules.Attendance.Infrastructure.Database;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastructure.Inbox;

internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> decorated,
    AttendanceDbContext attendanceDbContext)
    : IntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public override async Task Handle(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        var inboxMessageConsumer = new InboxMessageConsumer(integrationEvent.Id, decorated.GetType().Name);

        if (await InboxConsumerExistsAsync(inboxMessageConsumer, cancellationToken))
        {
            return;
        }

        await decorated.Handle(integrationEvent, cancellationToken);

        await InsertInboxConsumerAsync(inboxMessageConsumer, cancellationToken);
    }

    private async Task<bool> InboxConsumerExistsAsync(
        InboxMessageConsumer inboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        return await attendanceDbContext.Set<InboxMessageConsumer>()
            .AnyAsync(
                i => i.InboxMessageId == inboxMessageConsumer.InboxMessageId &&
                     i.Name == inboxMessageConsumer.Name,
                cancellationToken);
    }

    private async Task InsertInboxConsumerAsync(
        InboxMessageConsumer inboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        attendanceDbContext.Set<InboxMessageConsumer>().Add(inboxMessageConsumer);

        await attendanceDbContext.SaveChangesAsync(cancellationToken);
    }
}
