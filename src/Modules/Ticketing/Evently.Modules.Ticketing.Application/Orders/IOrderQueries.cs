using Evently.Modules.Ticketing.Application.Orders.ViewModels;

namespace Evently.Modules.Ticketing.Application.Orders;

public interface IOrderQueries
{
    Task<GetOrderViewModel> GetOrderAsync(Guid orderId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<GetOrdersViewModel>> GetOrdersAsync(Guid customerId, CancellationToken cancellationToken);
}
