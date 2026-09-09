using Evently.Modules.Ticketing.Application.Customers;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Repositories;

internal sealed class CustomerRepository(TicketingDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Customers.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Insert(Customer customer)
    {
        context.Customers.Add(customer);
    }
}
