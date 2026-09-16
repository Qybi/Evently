using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicket;

public sealed record GetTicketQuery(Guid TicketId) : IQuery<TicketViewModel>;
