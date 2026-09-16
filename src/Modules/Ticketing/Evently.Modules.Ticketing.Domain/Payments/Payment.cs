using Evently.Shared.Domain;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments.Errors;
using Evently.Modules.Ticketing.Domain.Payments.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Payments;

public sealed class Payment : Entity
{
    private Payment()
    {
    }

    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid TransactionId { get; private set; }

    public decimal Amount { get; private set; }

    public string Currency { get; private set; }

    public decimal? AmountRefunded { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? RefundedAtUtc { get; private set; }

    public static Payment Create(Order order, Guid transactionId, decimal amount, string currency)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            TransactionId = transactionId,
            Amount = amount,
            Currency = currency,
            CreatedAtUtc = DateTime.UtcNow
        };

        payment.Raise(new PaymentCreatedDomainEvent(payment.Id));

        return payment;
    }

    public Result Refund(decimal refundAmount)
    {
        decimal refunded = AmountRefunded ?? decimal.Zero;

        if (refunded == Amount)
        {
            return Result.Failure(PaymentErrors.AlreadyRefunded);
        }

        if (refunded + refundAmount > Amount)
        {
            return Result.Failure(PaymentErrors.NotEnoughFunds);
        }

        AmountRefunded = refunded + refundAmount;

        if (AmountRefunded == Amount)
        {
            RefundedAtUtc = DateTime.UtcNow;
            Raise(new PaymentRefundedDomainEvent(Id, TransactionId, refundAmount));
        }
        else
        {
            Raise(new PaymentPartiallyRefundedDomainEvent(Id, TransactionId, refundAmount));
        }

        return Result.Success();
    }
}
