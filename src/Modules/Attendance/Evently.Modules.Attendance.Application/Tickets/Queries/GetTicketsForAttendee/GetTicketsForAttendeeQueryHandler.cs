using Evently.Modules.Attendance.Application.Tickets.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Attendance.Application.Tickets.Queries.GetTicketsForAttendee;

internal sealed class GetTicketsForAttendeeQueryHandler(ITicketQueries ticketQueries)
    : IQueryHandler<GetTicketsForAttendeeQuery, IReadOnlyCollection<TicketViewModel>>
{
    public async Task<Result<IReadOnlyCollection<TicketViewModel>>> Handle(
        GetTicketsForAttendeeQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<TicketViewModel> tickets = await ticketQueries.GetTicketsForAttendeeAsync(
            request.AttendeeId,
            cancellationToken);

        return Result.Success(tickets);
    }
}
