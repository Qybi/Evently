using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Customers.Commands.UpdateCustomer;

public sealed record UpdateCustomerCommand(Guid CustomerId, string FirstName, string LastName) : ICommand;
