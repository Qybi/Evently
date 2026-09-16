using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.TicketTypes.Commands.UpdateTicketTypePrice;

public sealed record UpdateTicketTypePriceCommand(Guid TicketTypeId, decimal Price) : ICommand;
