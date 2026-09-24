using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Application.Tickets;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Attendees.Errors;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Domain.Tickets.Errors;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Microsoft.Extensions.Logging;

namespace Evently.Modules.Attendance.Application.Attendees.Commands.CheckInAttendee;

internal sealed class CheckInAttendeeCommandHandler(
    IAttendeeRepository attendeeRepository,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    ILogger<CheckInAttendeeCommandHandler> logger)
    : ICommandHandler<CheckInAttendeeCommand>
{
    public async Task<Result> Handle(CheckInAttendeeCommand request, CancellationToken cancellationToken)
    {
        Attendee? attendee = await attendeeRepository.GetAsync(request.AttendeeId, cancellationToken);

        if (attendee is null)
        {
            return Result.Failure(AttendeeErrors.NotFound(request.AttendeeId));
        }

        Ticket? ticket = await ticketRepository.GetAsync(request.TicketId, cancellationToken);

        if (ticket is null)
        {
            return Result.Failure(TicketErrors.NotFound(request.TicketId));
        }

        Result result = attendee.CheckIn(ticket);

        // Saved on failure too, so the invalid/duplicate check-in domain events get dispatched
        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Check in failed: {AttendeeId}, {TicketId}, {@Error}",
                attendee.Id,
                ticket.Id,
                result.Error);
        }

        return result;
    }
}
