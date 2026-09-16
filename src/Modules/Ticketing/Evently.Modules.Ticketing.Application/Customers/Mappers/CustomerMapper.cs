using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;
using Evently.Modules.Ticketing.Domain.Customers;
using Riok.Mapperly.Abstractions;

namespace Evently.Modules.Ticketing.Application.Customers.Mappers;

[Mapper]
public static partial class CustomerMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial CustomerViewModel ToViewModel(this Customer customer);
    public static partial IQueryable<CustomerViewModel> ProjectToViewModel(this IQueryable<Customer> customers);
}
