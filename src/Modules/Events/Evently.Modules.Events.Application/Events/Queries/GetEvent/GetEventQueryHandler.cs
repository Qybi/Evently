using Evently.Modules.Events.Application.Events.DTO;
using MediatR;

namespace Evently.Modules.Events.Application.Events.Queries.GetEvent;

internal sealed class GetEventQueryHandler(IEventQueries eventQueries) : IRequestHandler<GetEventQuery, EventResponseDto?>
{
    public Task<EventResponseDto?> Handle(GetEventQuery query, CancellationToken cancellationToken) =>
        eventQueries.GetAsync(query.EventId, cancellationToken);
}
