using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Events.Commands.RescheduleEvent;

public sealed record RescheduleEventCommand(Guid EventId, DateTime StartsAtUtc, DateTime? EndsAtUtc) : ICommand;
