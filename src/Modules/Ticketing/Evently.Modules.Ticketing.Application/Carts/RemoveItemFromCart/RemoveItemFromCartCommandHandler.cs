using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Application.Customers;
using Evently.Modules.Ticketing.Domain.TicketTypes.Errors;
using Evently.Modules.Ticketing.Domain.TicketTypes;
using Evently.Modules.Ticketing.Application.TicketTypes;

namespace Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;

internal sealed class RemoveItemFromCartCommandHandler(
    ICustomerRepository customerRepository,
    ITicketTypeRepository ticketTypeRepository,
    CartService cartService)
    : ICommandHandler<RemoveItemFromCartCommand>
{
    public async Task<Result> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        TicketType? ticketType = await ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));
        }

        await cartService.RemoveItemAsync(customer.Id, ticketType.Id, cancellationToken);

        return Result.Success();
    }
}
