using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.TicketTypes.Queries.GetTicketType;

public sealed record GetTicketTypeQuery(Guid TicketTypeId) : IQuery<TicketTypeViewModel>;
