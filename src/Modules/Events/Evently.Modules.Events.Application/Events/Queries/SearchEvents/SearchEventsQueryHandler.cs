using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Modules.Events.Domain.Abstractions;

namespace Evently.Modules.Events.Application.Events.Queries.SearchEvents;

internal sealed class SearchEventsQueryHandler(IEventQueries eventQueries)
    : IQueryHandler<SearchEventsQuery, SearchEventsViewModel>
{
    public async Task<Result<SearchEventsViewModel>> Handle(
        SearchEventsQuery request,
        CancellationToken cancellationToken)
    {
        SearchEventsViewModel events = await eventQueries.SearchAsync(
            request.CategoryId,
            request.StartDate,
            request.EndDate,
            request.Page,
            request.PageSize,
            cancellationToken);

        return events;
    }
}
