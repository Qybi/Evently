using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Tickets.Commands.CreateTicket;

public sealed record CreateTicketCommand(Guid TicketId, Guid AttendeeId, Guid EventId, string Code) : ICommand;
