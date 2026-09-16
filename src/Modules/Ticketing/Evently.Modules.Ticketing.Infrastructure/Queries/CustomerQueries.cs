using Evently.Modules.Ticketing.Application.Customers;
using Evently.Modules.Ticketing.Application.Customers.Mappers;
using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Ticketing.Infrastructure.Queries;

internal sealed class CustomerQueries(TicketingDbContext context) : ICustomerQueries
{
    public Task<CustomerViewModel?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return context.Customers
            .AsNoTracking()
            .Where(c => c.Id == id)
            .ProjectToViewModel()
            .SingleOrDefaultAsync(cancellationToken);
    }
}
