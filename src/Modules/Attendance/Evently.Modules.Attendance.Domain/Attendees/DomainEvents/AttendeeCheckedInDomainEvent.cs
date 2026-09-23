using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

public sealed class AttendeeCheckedInDomainEvent(Guid attendeeId, Guid eventId) : DomainEvent
{
    public Guid AttendeeId { get; init; } = attendeeId;

    public Guid EventId { get; init; } = eventId;
}
