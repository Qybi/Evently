using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Events;
using Evently.Modules.Events.Application.Events.Mappers;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Modules.Events.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Queries;

internal sealed class EventQueries(EventsDbContext context) : IEventQueries
{
    // Read side: no tracking, projected in SQL, never materializes the entity.
    public Task<EventViewModel?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Events
            .AsNoTracking()
            .Where(e => e.Id == id)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<EventViewModel>> GetEventsAsync(CancellationToken cancellationToken = default)
    {
        return await context.Events
            .AsNoTracking()
            .ProjectToViewModel()
            .ToListAsync(cancellationToken);
    }

    public async Task<SearchEventsViewModel> SearchAsync(
        Guid? categoryId,
        DateTime? startDate,
        DateTime? endDate,
        int page = 1,
        int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Event> query = context.Events
            .AsNoTracking()
            .Where(e => e.Status == EventStatus.Published);

        if (categoryId.HasValue)
        {
            query = query.Where(e => e.CategoryId == categoryId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(e => e.StartsAtUtc >= startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
            query = query.Where(e => e.EndsAtUtc <= endDate.Value.Date);
        }

        int totalCount = await query.CountAsync(cancellationToken);

        List<EventViewModel> events = await query
            .OrderBy(e => e.StartsAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectToViewModel()
            .ToListAsync(cancellationToken);

        return new SearchEventsViewModel(page, pageSize, totalCount, events);
    }
}
