using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Events.Presentation.Events.CancelEventSaga;

public sealed class CancelEventSaga : MassTransitStateMachine<CancelEventState>
{
    // possible states of the state machine (Initial and Final are built into MassTransit)
    public State CancellationStarted { get; private set; }
    public State PaymentsRefunded { get; private set; }
    public State TicketsArchived { get; private set; }

    // events that change the state machine's state
    // the first 3 are integration events received from the bus,
    // EventCancellationCompleted is raised internally by MassTransit (see CompositeEvent below)
    public Event<EventCanceledIntegrationEvent> EventCanceled { get; private set; }
    public Event<EventPaymentsRefundedIntegrationEvent> EventPaymentsRefunded { get; private set; }
    public Event<EventTicketsArchivedIntegrationEvent> EventTicketsArchived { get; private set; }
    public Event EventCancellationCompleted { get; private set; }

    public CancelEventSaga()
    {
        // correlation: tells MassTransit which saga instance an incoming message belongs to (message EventId = saga CorrelationId).
        // for EventCanceled, which starts the saga, EventId becomes the new instance's CorrelationId -> one saga per canceled event
        Event(() => EventCanceled, c => c.CorrelateById(m => m.Message.EventId));
        Event(() => EventPaymentsRefunded, c => c.CorrelateById(m => m.Message.EventId));
        Event(() => EventTicketsArchived, c => c.CorrelateById(m => m.Message.EventId));

        InstanceState(s => s.CurrentState);

        // the saga instance doesn't exist yet: EventCanceled creates it
        // initially means: "While in the initial state..."
        Initially(
            // "...when i encounter the EventCanceled event..."
            When(EventCanceled)
                // "...publish the EventCancellationStartedIntegrationEvent..."
                .Publish((context) => new EventCancellationStartedIntegrationEvent(context.Message.Id, context.Message.OccurredOnUtc, context.Message.EventId))
                // "...and transition to the CancellationStarted state"
                .TransitionTo(CancellationStarted)
            );

        // during cancellation started state, transition to either one of the 2 states depending on which one happens first
        During(CancellationStarted,
            When(EventPaymentsRefunded)
                .TransitionTo(PaymentsRefunded),
            When(EventTicketsArchived)
                .TransitionTo(TicketsArchived));

        // one step is done, wait for the other one: which During applies depends on which step completed first.
        During(PaymentsRefunded,
            When(EventTicketsArchived)
                .TransitionTo(TicketsArchived));

        During(TicketsArchived,
            When(EventPaymentsRefunded)
                .TransitionTo(PaymentsRefunded));

        // MassTransit declaration method to "when both events have happened - in any order -, raise the event specified in the first delegate"
        CompositeEvent(
            () => EventCancellationCompleted,
            // int bitmask persisted on the saga instance (bit 0 = PaymentsRefunded, bit 1 = TicketsArchived).
            // each message is handled in a separate saga load, so MassTransit must store which events already arrived
            state => state.CancellationCompletedStatus,
            EventPaymentsRefunded, EventTicketsArchived);

        // in any state (except Initial/Final), when the composite EventCancellationCompleted event fires: publish completion and move to Final
        DuringAny(
            When(EventCancellationCompleted)
                .Publish(context =>
                    new EventCancellationCompletedIntegrationEvent(
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        context.Saga.CorrelationId))
                .Finalize());

        // we keep this one commented since we want full history saved on redis with the status marked as "Final", method deletes sagas once they're marked as final. Useful to release memory
        #pragma warning disable S125
        //SetCompletedWhenFinalized();
        #pragma warning restore S125
    }
}
