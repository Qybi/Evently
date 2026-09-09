using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Customers.Queries.GetCustomer;

public sealed record GetCustomerByIdQuery(Guid CustomerId) : IQuery<CustomerViewModel>;
