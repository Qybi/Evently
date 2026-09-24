using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.Commands.UpdateAttendee;

public sealed record UpdateAttendeeCommand(Guid AttendeeId, string FirstName, string LastName) : ICommand;
