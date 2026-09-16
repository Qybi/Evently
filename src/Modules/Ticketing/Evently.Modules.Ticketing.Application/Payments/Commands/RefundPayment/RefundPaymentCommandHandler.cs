using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Domain.Payments.Errors;

namespace Evently.Modules.Ticketing.Application.Payments.Commands.RefundPayment;

internal sealed class RefundPaymentCommandHandler(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RefundPaymentCommand>
{
    public async Task<Result> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        Payment? payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken);

        if (payment is null)
        {
            return Result.Failure(PaymentErrors.NotFound(request.PaymentId));
        }

        Result result = payment.Refund(request.Amount);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
