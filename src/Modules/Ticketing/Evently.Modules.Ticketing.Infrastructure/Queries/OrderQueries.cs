using Evently.Modules.Ticketing.Application.Orders;
using Evently.Modules.Ticketing.Application.Orders.Mappers;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Queries;

internal sealed class OrderQueries(TicketingDbContext context) : IOrderQueries
{
    public async Task<GetOrderViewModel> GetOrderAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await context.Orders
             .Include(o => o.OrderItems)
             .AsNoTracking()
             .Where(o => o.Id == orderId)
             .ProjectToGetOrderViewModel()
             .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<GetOrdersViewModel>> GetOrdersAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .ProjectToGetOrdersViewModel()
            .ToListAsync(cancellationToken);
    }
}
