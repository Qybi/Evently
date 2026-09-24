using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Modules.Attendance.Application.Tickets.Queries.GetTicketsForAttendee;
using Evently.Modules.Attendance.Application.Tickets.Queries.ViewModels;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Attendance.Presentation.Tickets;

internal sealed class GetAttendeeTickets : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("attendees/tickets", async (IAttendanceContext attendanceContext, ISender sender) =>
        {
            Result<IReadOnlyCollection<TicketViewModel>> result = await sender.Send(
                new GetTicketsForAttendeeQuery(attendanceContext.AttendeeId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization("tickets:read")
        .WithTags(Tags.Tickets);
    }
}
