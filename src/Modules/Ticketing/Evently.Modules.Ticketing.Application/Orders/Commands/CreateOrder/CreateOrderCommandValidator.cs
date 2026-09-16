using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Orders.Commands.CreateOrder;

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
    }
}
