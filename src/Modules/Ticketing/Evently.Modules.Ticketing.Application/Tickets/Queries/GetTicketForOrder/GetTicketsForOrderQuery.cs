using Evently.Shared.Application.Messaging;
using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;

namespace Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicketForOrder;

public sealed record GetTicketsForOrderQuery(Guid OrderId) : IQuery<IReadOnlyCollection<TicketViewModel>>;
