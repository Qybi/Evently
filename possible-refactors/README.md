# Possible refactors

Ideas that were considered but not applied yet. Each entry explains the current state, the proposed change, and the trade-offs, with an implementation snippet ready to copy.

| Refactor | What it removes |
| --- | --- |
| [01 – Shared idempotent domain event handler](01-shared-idempotent-domain-event-handler.md) | The per-module copies of `IdempotentDomainEventHandler<T>` and of the `AddDomainEventHandlers` registration |
