using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Payments.Commands.RefundPayment;

public sealed record RefundPaymentCommand(Guid PaymentId, decimal Amount) : ICommand;
