using Evently.Shared.Application.Messaging;
using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;

namespace Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicketByCode;

public sealed record GetTicketByCodeQuery(string Code) : IQuery<TicketViewModel>;
