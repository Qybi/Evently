using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Orders.Queries.GetOrder;

public sealed record GetOrderQuery(Guid OrderId) : IQuery<GetOrderViewModel>;
