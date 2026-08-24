using Evently.Modules.Events.Application.Events.Queries.GetEvent;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Modules.Events.Presentation.ApiResults;
using Evently.Shared.Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id}", async (Guid id, ISender sender) =>
        {
            Result<EventViewModel> result = await sender.Send(new GetEventQuery(id));

            return result.Match(Results.Ok, ApiResults.ApiResults.Problem);
        })
        .WithTags(Tags.Events);
    }
}
