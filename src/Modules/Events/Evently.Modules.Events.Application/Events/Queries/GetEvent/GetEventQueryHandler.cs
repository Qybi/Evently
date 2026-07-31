using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Events.Errors;

namespace Evently.Modules.Events.Application.Events.Queries.GetEvent;

internal sealed class GetEventQueryHandler(IEventQueries eventQueries) : IQueryHandler<GetEventQuery, EventViewModel>
{
    public async Task<Result<EventViewModel>> Handle(GetEventQuery query, CancellationToken cancellationToken)
    {
        EventViewModel? eventViewModel = await eventQueries.GetAsync(query.EventId, cancellationToken);

        if (eventViewModel is null)
        {
            return Result.Failure<EventViewModel>(EventErrors.NotFound(query.EventId));
        }

        // implicitly return a successful result with the event view model (check Result implicit operator)
        return eventViewModel;
    }
}
