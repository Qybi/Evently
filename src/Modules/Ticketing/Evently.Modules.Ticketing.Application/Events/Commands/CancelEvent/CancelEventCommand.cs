using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Events.Commands.CancelEvent;

public sealed record CancelEventCommand(Guid EventId) : ICommand;
