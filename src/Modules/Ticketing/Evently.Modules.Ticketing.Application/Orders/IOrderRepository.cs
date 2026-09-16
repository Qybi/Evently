using Evently.Modules.Ticketing.Domain.Orders;

namespace Evently.Modules.Ticketing.Application.Orders;

public interface IOrderRepository
{
    Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Order order);
}
