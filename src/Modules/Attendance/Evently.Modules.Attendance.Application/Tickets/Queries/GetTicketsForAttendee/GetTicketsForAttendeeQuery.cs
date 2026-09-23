using Evently.Modules.Attendance.Application.Tickets.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Tickets.Queries.GetTicketsForAttendee;

public sealed record GetTicketsForAttendeeQuery(Guid AttendeeId) : IQuery<IReadOnlyCollection<TicketViewModel>>;
