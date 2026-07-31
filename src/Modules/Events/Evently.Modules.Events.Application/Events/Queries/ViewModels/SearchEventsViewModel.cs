namespace Evently.Modules.Events.Application.Events.Queries.ViewModels;

public sealed record SearchEventsViewModel(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<EventViewModel> Events);
