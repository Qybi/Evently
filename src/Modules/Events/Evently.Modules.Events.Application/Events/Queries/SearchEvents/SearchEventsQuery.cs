using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;

namespace Evently.Modules.Events.Application.Events.Queries.SearchEvents;

public sealed record SearchEventsQuery(
    Guid? CategoryId,
    DateTime? StartDate,
    DateTime? EndDate,
    int Page,
    int PageSize) : IQuery<SearchEventsViewModel>;
