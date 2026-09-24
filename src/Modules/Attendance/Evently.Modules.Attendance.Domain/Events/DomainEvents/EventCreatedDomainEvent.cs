using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Events.DomainEvents;

public sealed class EventCreatedDomainEvent(
    Guid eventId,
    string title,
    string description,
    string location,
    DateTime startsAtUtc,
    DateTime? endsAtUtc) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;

    public string Title { get; init; } = title;

    public string Description { get; init; } = description;

    public string Location { get; init; } = location;

    public DateTime StartsAtUtc { get; init; } = startsAtUtc;

    public DateTime? EndsAtUtc { get; init; } = endsAtUtc;
}
