using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Payments.Commands.RefundPaymentsForEvent;

public sealed record RefundPaymentsForEventCommand(Guid EventId) : ICommand;
