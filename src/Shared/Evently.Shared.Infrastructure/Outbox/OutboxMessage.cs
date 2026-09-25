namespace Evently.Shared.Infrastructure.Outbox;

// Multiple bootstrappers idea: add a BootstrapperId setting + column to each module's outbox_messages.
// Each executable writes its own id and polls only its own rows, so the process that raised a domain event
// is the one that handles it (a different exe with a different handler set can't mark it processed).
// Use a stable id from config (not machine/pid), keep FOR UPDATE SKIP LOCKED for scaled instances of the same exe,
// and index (bootstrapper_id, processed_on_utc). Cross-exe reactions belong to integration events, not domain events.
public sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; }
    public string Content { get; init; }
    public DateTime OccurredOnUtc { get; init; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}
