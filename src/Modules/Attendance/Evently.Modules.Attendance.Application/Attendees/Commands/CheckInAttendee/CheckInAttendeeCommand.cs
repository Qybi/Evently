using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.Commands.CheckInAttendee;

public sealed record CheckInAttendeeCommand(Guid AttendeeId, Guid TicketId) : ICommand;
