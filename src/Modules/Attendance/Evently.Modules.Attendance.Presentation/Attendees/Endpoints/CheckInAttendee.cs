using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Modules.Attendance.Application.Attendees.Commands.CheckInAttendee;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Attendance.Presentation.Attendees.Endpoints;

internal sealed class CheckInAttendee : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // The attendee is always the authenticated user: never bind AttendeeId from the request body
        app.MapPut("attendees/check-in", async (Request request, IAttendanceContext attendanceContext, ISender sender) =>
        {
            Result result = await sender.Send(
                new CheckInAttendeeCommand(attendanceContext.AttendeeId, request.TicketId));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.CheckInTicket)
        .WithTags(Tags.Attendees);
    }

    internal sealed class Request
    {
        public Guid TicketId { get; init; }
    }
}
