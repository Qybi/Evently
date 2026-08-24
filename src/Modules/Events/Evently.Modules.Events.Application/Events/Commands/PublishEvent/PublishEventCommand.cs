using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.Commands.PublishEvent;

public sealed record PublishEventCommand(Guid EventId) : ICommand;
