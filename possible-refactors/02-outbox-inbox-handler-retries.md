# 02 – Outbox/inbox handler retries

[← Back to index](README.md)

**Status:** not applied (proposed 2026-09-29).

## Current state

Every module has a `ProcessOutboxJob` and a `ProcessInboxJob`. Both run all handlers of a message inside a single `try`:

```csharp
try
{
    IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, SerializerSettings.Instance)!;

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
    logger.LogError(...);
    exception = caughtException;
}

outboxMessage.ProcessedOnUtc = dateTimeProvider.UtcNow;
outboxMessage.Error = exception?.ToString();
```

The poll query only selects rows `WHERE processed_on_utc IS NULL`, so a message stamped here is never picked up again.

### How retries happen today

| Situation | What happens |
| --- | --- |
| A handler throws | The rest of the loop is skipped. The message still gets `ProcessedOnUtc` and `Error`. **Never retried.** Handlers after the failing one never run. |
| The job's final `SaveChangesAsync`/`CommitAsync` fails, or the process dies mid-batch | The job's transaction rolls back, no `ProcessedOnUtc` is saved, and the next tick picks the same messages again. Handlers that already finished were recorded by their idempotent decorator in a separate transaction, so they are skipped. **This is the only automatic retry path.** |
| Shutdown cancels the token | A handler throws `OperationCanceledException`, the catch marks the message, but the final `SaveChangesAsync(cancellationToken)` throws on the cancelled token. The batch rolls back and is reprocessed after restart. |
| Manual retry | Reset the row: `UPDATE users.outbox_messages SET processed_on_utc = NULL, error = NULL WHERE id = '...';`. The next tick reprocesses it, and the decorators skip handlers that already succeeded. |

Other layers don't retry either:

- **MassTransit:** no `UseMessageRetry` or redelivery is configured. If `IntegrationEventConsumer` fails to insert into the inbox, the message faults. With the in-memory transport it goes to an in-memory `_error` queue that is lost on restart.
- **Quartz:** it doesn't retry a failed `Execute`. The simple trigger fires again on the next interval anyway, which only matters for the rollback case above.

### Why it matters

A transient error inside a handler (DB blip, failed publish) loses that handler's work for good, plus the work of every handler after it. The only trace is the `error` column and a log line.

Example: `OrderCreatedDomainEvent` has three handlers in Ticketing (`CreateTicketsDomainEventHandler`, `OrderCreatedDomainEventHandler`, `SendOrderConfirmationDomainEventHandler`). If `CreateTicketsDomainEventHandler` fails on a DB blip and runs first, `OrderCreatedIntegrationEvent` is never published.

## Proposed change

1. **One scope per handler, with its own try/catch.** A failing handler no longer stops the ones after it. A separate scope also means its half-done DbContext changes can't be saved by the next handler's `SaveChanges`. (Today the whole message shares one scope, which is fine only because the loop stops at the first failure.)
2. **Leave `ProcessedOnUtc` null while any handler has failed.** The next tick retries the message. The idempotent decorators skip the handlers that already succeeded, so only the failed ones run again.
3. **Add a `RetryCount` column and a `MaxRetries` option.** When `RetryCount` reaches the limit, stamp `ProcessedOnUtc` and keep `Error`. That is already what "failed" means today, so the poll query and the meaning of a stamped row with an error don't change.
4. **If the JSON can't be deserialized, give up on the message straight away.** Retrying can't fix bad content.
5. **Stop running handlers once the token is cancelled.** A `when (!cancellationToken.IsCancellationRequested)` filter lets `OperationCanceledException` escape the loop. The final `SaveChangesAsync(cancellationToken)` would throw on the cancelled token anyway, so this isn't needed to avoid counting a wasted retry. It only avoids starting more handlers during shutdown.

### `OutboxMessage` / `InboxMessage`

```csharp
public int RetryCount { get; set; }
```

### `DomainEventHandlersFactory` / `IntegrationEventHandlersFactory`

Today the factory returns instances, all resolved from one scope. Add a method that returns only the cached types, so the job can resolve each handler in its own scope. `GetHandlers` keeps working on top of it.

```csharp
public static Type[] GetHandlerTypes(Type type, Assembly assembly)
{
    return HandlersDictionary.GetOrAdd(
        (assembly, type),
        _ => assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler<>).MakeGenericType(type)))
            .ToArray());
}

public static IEnumerable<IDomainEventHandler> GetHandlers(Type type, IServiceProvider serviceProvider, Assembly assembly)
{
    List<IDomainEventHandler> handlers = [];
    foreach (Type domainEventHandlerType in GetHandlerTypes(type, assembly))
    {
        object domainEventHandler = serviceProvider.GetRequiredService(domainEventHandlerType);

        handlers.Add((domainEventHandler as IDomainEventHandler)!);
    }

    return handlers;
}
```

`IntegrationEventHandlersFactory` gets the same split with `IIntegrationEventHandler<>`.

### `ProcessOutboxJob` loop (Users shown)

```csharp
foreach (OutboxMessage outboxMessage in outboxMessages)
{
    IDomainEvent domainEvent;
    try
    {
        domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, SerializerSettings.Instance)!;
    }
    catch (JsonException exception)
    {
        // Retrying can't fix bad content: give up on it now.
        logger.LogError(exception, "{Module} - Cannot deserialize outbox message {MessageId}", ModuleName, outboxMessage.Id);
        outboxMessage.ProcessedOnUtc = dateTimeProvider.UtcNow;
        outboxMessage.Error = exception.ToString();
        continue;
    }

    List<Exception> failures = [];

    foreach (Type handlerType in DomainEventHandlersFactory.GetHandlerTypes(domainEvent.GetType(), Application.AssemblyReference.Assembly))
    {
        // Own scope per handler: a failed handler's tracked changes must not reach the next handler's SaveChanges.
        await using AsyncServiceScope scope = serviceScopeFactory.CreateAsyncScope();
        try
        {
            var handler = (IDomainEventHandler)scope.ServiceProvider.GetRequiredService(handlerType);
            await handler.Handle(domainEvent, cancellationToken);
        }
        catch (Exception exception) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogError(exception, "{Module} - Handler {Handler} failed for outbox message {MessageId}",
                ModuleName, handlerType.Name, outboxMessage.Id);
            failures.Add(exception);
        }
    }

    if (failures.Count == 0)
    {
        outboxMessage.ProcessedOnUtc = dateTimeProvider.UtcNow;
        outboxMessage.Error = null;
        continue;
    }

    outboxMessage.RetryCount++;
    outboxMessage.Error = new AggregateException(failures).ToString();

    if (outboxMessage.RetryCount >= outboxOptions.Value.MaxRetries)
    {
        // Give up: stop polling it, keep Error for inspection.
        outboxMessage.ProcessedOnUtc = dateTimeProvider.UtcNow;
    }
}
```

`ProcessInboxJob` is the same with `InboxMessage`, `IIntegrationEvent`, `IntegrationEventHandlersFactory`, `Presentation.AssemblyReference.Assembly` and `inboxOptions`.

### `OutboxOptions` / `InboxOptions`

```csharp
public int MaxRetries { get; init; }
```

### Configuration (`modules.<module>.json` and `modules.<module>.Development.json`)

```json
"Outbox": {
  "IntervalInSeconds": 5,
  "BatchSize": 50,
  "MaxRetries": 5
},
"Inbox": {
  "IntervalInSeconds": 5,
  "BatchSize": 50,
  "MaxRetries": 5
}
```

At a 5-second interval, 5 retries give about 25 seconds to ride out a short DB outage (about 75 seconds with the 15-second Development interval).

## Steps to apply

1. Add `RetryCount` to `OutboxMessage` and `InboxMessage` in `Evently.Shared.Infrastructure`.
2. Split `GetHandlerTypes` out of `DomainEventHandlersFactory` and `IntegrationEventHandlersFactory`.
3. In each of the four modules:
   - add `MaxRetries` to `Outbox/OutboxOptions.cs` and `Inbox/InboxOptions.cs`;
   - replace the loop in `Outbox/ProcessOutboxJob.cs` and `Inbox/ProcessInboxJob.cs`;
   - add a migration from `src/API/Evently.Api`, e.g. for Users:
     ```
     dotnet ef migrations add AddMessageRetryCount -c UsersDbContext -o Database\Migrations -p ..\..\Modules\Users\Evently.Modules.Users.Infrastructure\Evently.Modules.Users.Infrastructure.csproj
     ```
     Existing rows get `retry_count = 0` from the column default.
4. Add `MaxRetries` to the 8 `modules.*.json` files.
5. Build, run the architecture tests, and check a forced handler failure: the message should stay unprocessed with `retry_count` climbing, then be stamped with its `error` once it hits the limit.

Suggested order: do Users first end to end, then copy to the other three modules.

## What does not change

- **Poll query.** Still `WHERE processed_on_utc IS NULL ORDER BY occurred_on_utc`.
- **Meaning of a stamped row with an error.** Still "failed, not retried". It now means "failed `MaxRetries` times" instead of "failed once".
- **Idempotency.** Still provided by `outbox_message_consumers` / `inbox_message_consumers`. Nothing new is recorded per handler.
- **Job transaction.** Still one transaction per batch with `FOR UPDATE`. Handler scopes still use their own connections.

## Trade-offs

- **No backoff.** A failing message is retried on every tick until it hits the limit. For spacing, add a `next_attempt_on_utc` column and filter on it in the poll query. Not worth it until retries prove too aggressive.
- **Failing messages hold batch slots.** A failing message stays at the head of `ORDER BY occurred_on_utc` until it hits the limit, so it takes up to `MaxRetries` ticks of one slot. With `BatchSize` 20–50 that's negligible.
- **More scopes.** One scope per handler instead of one per message. The cost is one extra DbContext per handler per message, which is cheap next to the handler's own work.
- **At-least-once for handlers.** If a handler succeeds but its decorator fails to save the consumer row, the handler runs again on retry. This is already true today in the rollback case, but retries make it more frequent. Commands like `CreateTicketBatchCommand` must tolerate re-execution. Handlers that publish integration events are already safe, because they reuse `domainEvent.Id` as the integration event id, once the inbox duplicate-insert issue below is fixed.

## Alternatives considered

### Keep the loop, stop at the first failure, just don't stamp

Smallest diff: move the `ProcessedOnUtc` stamp into the success path and add `RetryCount`. On retry the decorators skip the handlers that already succeeded, so the failing handler is retried. But when the message hits `MaxRetries`, every handler **after** the failing one never runs, which is the same loss as today, just delayed. Per-handler isolation gives every handler its own chance to succeed.

### Retry inside the job (Polly or a loop around `Handle`)

Retrying in place blocks the batch and holds the `FOR UPDATE` locks while waiting. Retrying on the next tick spaces attempts out for free and survives restarts because the count is stored.

### MassTransit retry

Only covers the inbox *insert* (`IntegrationEventConsumer`), not handler execution, which happens later in `ProcessInboxJob`. It doesn't help the outbox at all.

## Related follow-ups

- **Duplicate inbox insert.** `IntegrationEventConsumer` does a plain `Add` + `SaveChangesAsync`. A republished integration event with the same id hits a primary-key violation on `inbox_messages` and the consumer faults. Retries make republishing more likely, so fix it alongside: check `AnyAsync` first, or catch Postgres `23505`.
- **Entry [01](01-shared-idempotent-domain-event-handler.md), "Related follow-up".** If `ProcessOutboxJob` becomes `ProcessOutboxJob<TDbContext>` in shared, this change is applied once there instead of eight times. Doing 01's follow-up first makes this one smaller.
