using Evently.Modules.Ticketing.Domain.Customers;

namespace Evently.Modules.Ticketing.Application.Customers;

public interface ICustomerRepository
{
    Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Customer customer);
}
