using Evently.Shared.Application.Messaging;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;

namespace Evently.Modules.Ticketing.Application.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery(Guid CustomerId) : IQuery<IReadOnlyCollection<GetOrdersViewModel>>;
