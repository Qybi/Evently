using Evently.Modules.Events.Application.Events.DTO;
using Evently.Modules.Events.Application.Events.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Api.Events;

internal static class GetEvent
{
    internal static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetEventQuery(id);
            EventResponseDto? @event = await sender.Send(query);

            return @event is null ? Results.NotFound() : Results.Ok(@event);
        }).WithTags(Tags.Events);
    }
}
