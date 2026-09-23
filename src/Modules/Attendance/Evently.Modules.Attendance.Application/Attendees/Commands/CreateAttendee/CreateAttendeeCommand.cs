using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.Commands.CreateAttendee;

public sealed record CreateAttendeeCommand(Guid AttendeeId, string Email, string FirstName, string LastName) : ICommand;
