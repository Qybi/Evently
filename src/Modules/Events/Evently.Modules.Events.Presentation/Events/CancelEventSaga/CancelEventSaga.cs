using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using Wolverine.Persistence.Sagas;

namespace Evently.Modules.Events.Presentation.Events.CancelEventSaga;

#pragma warning disable IDE0060 // Remove unused parameter
// Wolverine uses Orchestration Sagas, while previous definition used MassTransit state machine saga.
public sealed class CancelEventSaga : Wolverine.Saga
{
    public Guid Id { get; set; }

    public bool PaymentsRefunded { get; set; }
    public bool TicketsArchived { get; set; }

    public static (CancelEventSaga, EventCancellationStartedIntegrationEvent) Start(
        EventCanceledIntegrationEvent message)
    {
        var saga = new CancelEventSaga { Id = message.EventId };

        var started = new EventCancellationStartedIntegrationEvent(
            message.Id,
            message.OccurredOnUtc,
            message.EventId);

        return (saga, started);
    }

    // this methods updates the saga state 
    public EventCancellationCompletedIntegrationEvent? Handle(
        [SagaIdentityFrom(nameof(EventPaymentsRefundedIntegrationEvent.EventId))]
        EventPaymentsRefundedIntegrationEvent message)
    {
        PaymentsRefunded = true;
        return TryComplete();
    }

    public EventCancellationCompletedIntegrationEvent? Handle(
        [SagaIdentityFrom(nameof(EventTicketsArchivedIntegrationEvent.EventId))]
        EventTicketsArchivedIntegrationEvent message)
    {
        TicketsArchived = true;
        return TryComplete();
    }

    // methods to handle a case where the saga is not found, for example if the saga has already completed and been removed from the database
    // optional, but useful to log or trigger some self-check/compensating actions
    public static void NotFound(
        [SagaIdentityFrom(nameof(EventPaymentsRefundedIntegrationEvent.EventId))]
        EventPaymentsRefundedIntegrationEvent message)
    {
        // Handle not found
    }

    public static void NotFound(
        [SagaIdentityFrom(nameof(EventTicketsArchivedIntegrationEvent.EventId))]
        EventTicketsArchivedIntegrationEvent message)
    {
        // Handle not found
    }

    private EventCancellationCompletedIntegrationEvent? TryComplete()
    {
        if (!PaymentsRefunded || !TicketsArchived)
        {
            return null;
        }

        MarkCompleted();

        return new EventCancellationCompletedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            Id);
    }
}

#pragma warning restore IDE0060 // Remove unused parameter
