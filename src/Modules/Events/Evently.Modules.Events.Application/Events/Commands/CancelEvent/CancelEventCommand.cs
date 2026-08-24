using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.Commands.CancelEvent;

public sealed record CancelEventCommand(Guid EventId) : ICommand;
