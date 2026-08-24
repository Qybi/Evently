using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.Commands.RescheduleEvent;

public sealed record RescheduleEventCommand(Guid EventId, DateTime StartsAtUtc, DateTime? EndsAtUtc) : ICommand;
