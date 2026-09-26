# 01 – Shared idempotent domain event handler

[← Back to index](README.md)

**Status:** not applied (proposed 2026-09-25).

## Current state

Every module (Attendance, Events, Ticketing, Users) has two pieces of outbox plumbing that are identical except for the module's DbContext:

| Piece | Location (per module) |
| --- | --- |
| `IdempotentDomainEventHandler<TDomainEvent>` | `Evently.Modules.<Module>.Infrastructure/Outbox/IdempotentDomainEventHandler.cs` |
| `AddDomainEventHandlers()` (private) | `Evently.Modules.<Module>.Infrastructure/<Module>Module.cs` |

The decorator takes the module's DbContext (`UsersDbContext`, `EventsDbContext`, …). The registration method scans `Application.AssemblyReference.Assembly` and decorates every handler with the module's own `IdempotentDomainEventHandler<>`. That makes 8 near-identical blocks of code.

## Why the duplication existed

The original decorator used Dapper, and its SQL hardcoded the schema:

```sql
SELECT EXISTS(
    SELECT 1
    FROM users.outbox_message_consumers
    WHERE outbox_message_id = @OutboxMessageId AND name = @Name
)
```

A shared class couldn't know which schema to query, so each module needed its own decorator. Each module's registration then had to point at its own decorator type.

With EF Core that reason is gone. Every DbContext sets its schema through `modelBuilder.HasDefaultSchema(...)`, so `dbContext.Set<OutboxMessageConsumer>()` already resolves to `users.outbox_message_consumers`, `events.outbox_message_consumers`, and so on. The only thing that still varies per module is the **DbContext type**, and a generic parameter can carry it.

The scanned assembly also varies, but that is just an argument, the same way `DomainEventHandlersFactory.GetHandlers` already takes one.

## Proposed change

Move one decorator and one registration extension into `Evently.Shared.Infrastructure/Outbox`, both generic over `TDbContext`.

### `Evently.Shared.Infrastructure/Outbox/IdempotentDomainEventHandler.cs`

```csharp
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain.DomainEvents;
using Microsoft.EntityFrameworkCore;

namespace Evently.Shared.Infrastructure.Outbox;

internal sealed class IdempotentDomainEventHandler<TDomainEvent, TDbContext>(
    IDomainEventHandler<TDomainEvent> decorated,
    TDbContext dbContext)
    : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
    where TDbContext : DbContext
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var outboxMessageConsumer = new OutboxMessageConsumer(domainEvent.Id, decorated.GetType().Name);

        if (await OutboxConsumerExistsAsync(outboxMessageConsumer, cancellationToken))
        {
            return;
        }

        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxConsumerAsync(outboxMessageConsumer, cancellationToken);
    }

    private async Task<bool> OutboxConsumerExistsAsync(
        OutboxMessageConsumer outboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        return await dbContext.Set<OutboxMessageConsumer>()
            .AnyAsync(
                o => o.OutboxMessageId == outboxMessageConsumer.OutboxMessageId &&
                     o.Name == outboxMessageConsumer.Name,
                cancellationToken);
    }

    private async Task InsertOutboxConsumerAsync(
        OutboxMessageConsumer outboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        dbContext.Set<OutboxMessageConsumer>().Add(outboxMessageConsumer);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
```

### `Evently.Shared.Infrastructure/Outbox/DomainEventHandlerExtensions.cs`

This follows the naming of `EndpointExtensions.AddEndpoints` in `Evently.Shared.Presentation`.

```csharp
using System.Reflection;
using Evently.Shared.Application.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Shared.Infrastructure.Outbox;

public static class DomainEventHandlerExtensions
{
    public static IServiceCollection AddDomainEventHandlers<TDbContext>(
        this IServiceCollection services,
        Assembly assembly)
        where TDbContext : DbContext
    {
        Type[] domainEventHandlers = assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<,>)
                .MakeGenericType(domainEvent, typeof(TDbContext));

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }

        return services;
    }
}
```

### Module side

Each `<Module>Module.cs` swaps the private method for one call:

```csharp
public static IServiceCollection AddUsersModule(
    this IServiceCollection services,
    IConfiguration configuration)
{
    services.AddDomainEventHandlers<UsersDbContext>(Application.AssemblyReference.Assembly);

    services.AddInfrastructure(configuration);

    services.AddEndpoints(Presentation.AssemblyReference.Assembly);

    return services;
}
```

| Module | Call |
| --- | --- |
| Attendance | `services.AddDomainEventHandlers<AttendanceDbContext>(Application.AssemblyReference.Assembly);` |
| Events | `services.AddDomainEventHandlers<EventsDbContext>(Application.AssemblyReference.Assembly);` |
| Ticketing | `services.AddDomainEventHandlers<TicketingDbContext>(Application.AssemblyReference.Assembly);` |
| Users | `services.AddDomainEventHandlers<UsersDbContext>(Application.AssemblyReference.Assembly);` |

## Steps to apply

1. Add the two shared files above.
2. In each of the four `<Module>Module.cs` files:
   - replace `services.AddDomainEventHandlers();` with the generic call from the table;
   - delete the private `AddDomainEventHandlers` method;
   - remove the `using`s that become unused (`Evently.Shared.Application.Messaging`, `Microsoft.Extensions.DependencyInjection.Extensions`, unless something else in the file still needs them).
3. Delete the four `Evently.Modules.<Module>.Infrastructure/Outbox/IdempotentDomainEventHandler.cs` files.
4. Build and run the architecture tests.

## What does not change

- **Stored consumer names.** The consumer name is still `decorated.GetType().Name`, so existing `outbox_message_consumers` rows remain valid and no data migration is needed.
- **Schema.** Each module still writes to its own `<schema>.outbox_message_consumers` table. Each DbContext must still call `modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration())`, and each module still needs its own migration for the table.
- **Atomicity.** The decorated handler's work and the consumer row are still two separate saves, as before.

## Why it fits

- `Evently.Shared.Infrastructure` already references EF Core (`OutboxMessageConfiguration`) and Scrutor (`Decorate`), so no new package references are needed.
- All module DbContexts are `public sealed`, so they can be used as a type argument from the shared assembly.
- The decorator can stay `internal`: DI builds it through its public primary constructor, and `MakeGenericType` works on internal types.
- `AddDbContext` registers the DbContext as scoped, so it is resolved from the same scope that `ProcessOutboxJob` creates for each message.

## Alternatives considered

### One scan in the host for all modules

`Program.cs` could scan every module's Application assembly in a single call. Each assembly would still have to be paired with its own DbContext, so that mapping would have to be passed in anyway. It would also move registration out of the modules, which currently own their wiring. One line per module is simpler.

### Scrutor open-generic decoration

`services.Decorate(typeof(IDomainEventHandler<>), typeof(IdempotentDomainEventHandler<>))` decorates every registration of an **open generic service type**. Handlers here are registered by **concrete type** (`services.TryAddScoped(domainEventHandler)`), because `DomainEventHandlersFactory` resolves them by concrete type, and none of those concrete types is generic. An open-generic decorate would match nothing, so the loop is still needed. With this refactor it just lives in shared.

## Related follow-up

`ProcessOutboxJob` is duplicated per module for the same reason: its `FromSql` query hardcodes `users.outbox_messages`. It could also become `ProcessOutboxJob<TDbContext>` by reading the table name from the EF model:

```csharp
IEntityType entityType = dbContext.Model.FindEntityType(typeof(OutboxMessage))!;
string table = $"{entityType.GetSchema()}.{entityType.GetTableName()}";
```

Table names can't be SQL parameters, so the query would need `FromSqlRaw` with the table name concatenated in. That is safe because the name comes from the model, not from user input, but `BatchSize` should still be passed as a parameter. The job also has a per-module `ModuleName`, per-module `OutboxOptions` configuration, and Quartz job registration, so this is a bigger change and belongs in its own entry.
