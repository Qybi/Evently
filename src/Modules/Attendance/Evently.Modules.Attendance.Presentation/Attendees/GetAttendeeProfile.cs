using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Modules.Attendance.Application.Attendees.Queries.GetAttendee;
using Evently.Modules.Attendance.Application.Attendees.Queries.ViewModels;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Attendance.Presentation.Attendees;

internal sealed class GetAttendeeProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("attendees/profile", async (IAttendanceContext attendanceContext, ISender sender) =>
        {
            Result<AttendeeViewModel> result = await sender.Send(new GetAttendeeQuery(attendanceContext.AttendeeId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization("users:read")
        .WithTags(Tags.Attendees);
    }
}
