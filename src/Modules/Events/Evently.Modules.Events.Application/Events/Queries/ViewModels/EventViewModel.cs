namespace Evently.Modules.Events.Application.Events.Queries.ViewModels;

public sealed record EventViewModel(
    Guid Id,
    Guid CategoryId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc
);
