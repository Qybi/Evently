using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Tickets.Commands.CreateTicketBatch;

public sealed record CreateTicketBatchCommand(Guid OrderId) : ICommand;
