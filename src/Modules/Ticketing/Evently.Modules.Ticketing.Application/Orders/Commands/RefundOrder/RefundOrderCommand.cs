using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Orders.Commands.RefundOrder;

public sealed record RefundOrderCommand(Guid OrderId) : ICommand;
