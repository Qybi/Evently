using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Orders.Queries.GetOrders;

internal sealed class GetOrdersQueryHandler(IOrderQueries orderQueries) : IQueryHandler<GetOrdersQuery, IReadOnlyCollection<GetOrdersViewModel>>
{
    public async Task<Result<IReadOnlyCollection<GetOrdersViewModel>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<GetOrdersViewModel> orders = await orderQueries.GetOrdersAsync(request.CustomerId, cancellationToken);

        return Result.Success(orders);
    }
}
