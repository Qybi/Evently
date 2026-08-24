using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.TicketTypes.Commands.CreateTicketType;

public sealed record CreateTicketTypeCommand(
    Guid EventId,
    string Name,
    decimal Price,
    string Currency,
    decimal Quantity) : ICommand<Guid>;
