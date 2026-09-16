using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Tickets.Commands.ArchiveTicketsForEvent;

public sealed record ArchiveTicketsForEventCommand(Guid EventId) : ICommand;
