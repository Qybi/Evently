using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Events.Application.Events.Queries.GetEvents;

internal sealed class GetEventsQueryHandler(IEventQueries eventQueries)
    : IQueryHandler<GetEventsQuery, IReadOnlyCollection<EventViewModel>>
{
    public async Task<Result<IReadOnlyCollection<EventViewModel>>> Handle(
        GetEventsQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<EventViewModel> events = await eventQueries.GetEventsAsync(cancellationToken);

        return Result.Success(events);
    }
}
