using Evently.Modules.Attendance.Application.EventStatistics.Queries.GetEventStatistics;
using Evently.Modules.Attendance.Application.EventStatistics.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Attendance.Presentation.EventStatistics.Endpoints;

internal sealed class GetEventStatistics : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("event-statistics/{id}", async (
            Guid id,
            IQueryHandler<GetEventStatisticsQuery, GetEventStatisticsViewModel> handler,
            CancellationToken cancellationToken) =>
        {
            Result<GetEventStatisticsViewModel> result = await handler.Handle(new GetEventStatisticsQuery(id), cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetEventStatistics)
        .WithTags(Tags.EventStatistics);
    }
}
