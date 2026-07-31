using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;

namespace Evently.Modules.Events.Application.TicketTypes.Queries.GetTicketType;

public sealed record GetTicketTypeQuery(Guid TicketTypeId) : IQuery<TicketTypeViewModel>;
