namespace Evently.Modules.Events.Application.Events.DTO;

public sealed record EventResponseDto(
    Guid Id,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc
);
