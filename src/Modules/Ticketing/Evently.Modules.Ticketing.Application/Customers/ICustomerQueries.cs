using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;

namespace Evently.Modules.Ticketing.Application.Customers;

public interface ICustomerQueries
{
    Task<CustomerViewModel?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
