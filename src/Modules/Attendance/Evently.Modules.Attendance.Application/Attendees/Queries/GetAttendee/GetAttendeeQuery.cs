using Evently.Modules.Attendance.Application.Attendees.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.Queries.GetAttendee;

public sealed record GetAttendeeQuery(Guid AttendeeId) : IQuery<AttendeeViewModel>;
