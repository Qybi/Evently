using System.Collections.Concurrent;
using System.Reflection;
using Evently.Shared.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Shared.Infrastructure.Outbox;

public static class DomainEventHandlersFactory
{
    // Process-wide cache: (module assembly + event type) -> handler types. Concurrent because outbox jobs can run in parallel.
    private static readonly ConcurrentDictionary<(Assembly Assembly, Type DomainEventType), Type[]> HandlersDictionary = new();

    public static IEnumerable<IDomainEventHandler> GetHandlers(Type type, IServiceProvider serviceProvider, Assembly assembly)
    {
        // Assembly in the key scopes lookup to one module; reflection scan runs only on first miss.
        Type[] domainEventHandlerTypes = HandlersDictionary.GetOrAdd(
            (assembly, type),
            _ =>
            {
                // Close IDomainEventHandler<> over the runtime event type to find its handlers.
                Type[] domainEventHandlerTypes = assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler<>).MakeGenericType(type)))
                    .ToArray();
                return domainEventHandlerTypes;
            });

        // Cache holds types only; instances come from the caller's scope so scoped deps (DbContext) stay fresh.
        List<IDomainEventHandler> handlers = [];
        foreach (Type domainEventHandlerType in domainEventHandlerTypes)
        {
            // Handlers are registered by concrete type, so this may return the idempotent decorator instead.
            object domainEventHandler = serviceProvider.GetRequiredService(domainEventHandlerType);

            // Non-generic interface lets the caller invoke Handle(IDomainEvent) without knowing T.
            handlers.Add((domainEventHandler as IDomainEventHandler)!);
        }

        return handlers;
    }
}


#pragma warning disable S125 // Sections of code should not be commented out
/*
Alternative implementation (reference only):
- static lambda reads assembly/type from the key: no closure allocated per call
- closed IDomainEventHandler<T> built once per scan, not once per type in the assembly
- concrete-class filter: abstract bases would match IsAssignableTo but can't be resolved from DI
- hard cast: a bad registration fails with InvalidCastException here, not a null further down
- IReadOnlyList: result is materialized, caller can't mistake it for a lazy sequence

public static class DomainEventHandlersFactory
{
    private static readonly ConcurrentDictionary<(Assembly Assembly, Type EventType), Type[]> HandlerTypes = new();

    public static IReadOnlyList<IDomainEventHandler> GetHandlers(
        Type domainEventType,
        IServiceProvider serviceProvider,
        Assembly assembly)
    {
        Type[] handlerTypes = HandlerTypes.GetOrAdd((assembly, domainEventType), static key =>
        {
            Type closedHandlerInterface = typeof(IDomainEventHandler<>).MakeGenericType(key.EventType);

            return key.Assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(closedHandlerInterface))
                .ToArray();
        });

        return Array.ConvertAll(handlerTypes, t => (IDomainEventHandler)serviceProvider.GetRequiredService(t));
    }
}
*/
#pragma warning restore S125 // Sections of code should not be commented out
