using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Events.Commands.CreateEvent;

public sealed record CreateEventCommand(
    Guid EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc) : ICommand;
