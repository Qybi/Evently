using Evently.Modules.Ticketing.Application.Customers;
using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;
using Evently.Modules.Ticketing.Application.TicketTypes;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.TicketTypes;
using Evently.Modules.Ticketing.Domain.TicketTypes.Errors;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

public sealed record AddItemToCartCommand(Guid CustomerId, Guid TicketTypeId, decimal Quantity) : ICommand;

internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.TicketTypeId).NotEmpty();
        RuleFor(c => c.Quantity).GreaterThan(decimal.Zero);
    }
}

internal sealed class AddItemToCartCommandHandler(CartService cartService, ICustomerRepository customerRepository, ITicketTypeRepository ticketTypeRepository)
    : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
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

        var cartItem = new CartItem
        {
            TicketTypeId = ticketType.Id,
            Price = ticketType.Price,
            Quantity = request.Quantity,
            Currency = ticketType.Currency
        };

        await cartService.AddItemAsync(customer.Id, cartItem, cancellationToken);

        return Result.Success();
    }
}
