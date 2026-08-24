using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.Commands.CreateEvent;

public sealed record CreateEventCommand(
    Guid CategoryId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc) : ICommand<Guid>;

