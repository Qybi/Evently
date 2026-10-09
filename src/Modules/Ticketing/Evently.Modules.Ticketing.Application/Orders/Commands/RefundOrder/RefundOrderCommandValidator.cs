using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Orders.Commands.RefundOrder;

internal sealed class RefundOrderCommandValidator : AbstractValidator<RefundOrderCommand>
{
    public RefundOrderCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}
