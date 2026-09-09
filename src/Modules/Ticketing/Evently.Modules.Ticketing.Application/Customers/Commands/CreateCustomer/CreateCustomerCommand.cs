using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(Guid CustomerId, string Email, string FirstName, string LastName) : ICommand;
