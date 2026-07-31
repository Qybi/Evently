using Evently.Modules.Events.Application.Events.Queries.ViewModels;

namespace Evently.Modules.Events.Application.Events;

public interface IEventQueries
{
    Task<EventViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<EventViewModel>> GetEventsAsync(CancellationToken cancellationToken = default);
    Task<SearchEventsViewModel> SearchAsync(
        Guid? categoryId,
        DateTime? startDate,
        DateTime? endDate,
        int page = 1,
        int pageSize = 25,
        CancellationToken cancellationToken = default);
}
