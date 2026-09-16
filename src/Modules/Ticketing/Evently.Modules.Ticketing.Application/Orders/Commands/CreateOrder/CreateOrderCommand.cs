using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId) : ICommand;
