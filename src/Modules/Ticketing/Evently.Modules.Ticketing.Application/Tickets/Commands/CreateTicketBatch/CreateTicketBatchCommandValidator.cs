using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Tickets.Commands.CreateTicketBatch;

internal sealed class CreateTicketBatchCommandValidator : AbstractValidator<CreateTicketBatchCommand>
{
    public CreateTicketBatchCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}
