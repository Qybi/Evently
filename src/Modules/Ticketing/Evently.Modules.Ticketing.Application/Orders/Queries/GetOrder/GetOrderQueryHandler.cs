using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Modules.Ticketing.Domain.Orders.Errors;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Orders.Queries.GetOrder;

internal sealed class GetOrderQueryHandler(IOrderQueries orderQueries)
    : IQueryHandler<GetOrderQuery, GetOrderViewModel>
{
    public async Task<Result<GetOrderViewModel>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        GetOrderViewModel? orderViewModel = await orderQueries.GetOrderAsync(request.OrderId, cancellationToken);

        if (orderViewModel is null)
        {
            return Result.Failure<GetOrderViewModel>(OrderErrors.NotFound(request.OrderId));
        }

        return orderViewModel;
    }
}
