using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Ticketing.Application.Customers.Queries.GetCustomer;

internal sealed class GetCustomerByIdQueryHandler(ICustomerQueries customerQueries)
    : IQueryHandler<GetCustomerByIdQuery, CustomerViewModel>
{
    public async Task<Result<CustomerViewModel>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        CustomerViewModel? customer = await customerQueries.GetCustomerByIdAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure<CustomerViewModel>(CustomerErrors.NotFound(request.CustomerId));
        }

        return customer;
    }
}
