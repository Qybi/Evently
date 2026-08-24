using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.TicketTypes.Queries.GetEventTicketTypes;

public sealed record GetEventTicketTypesQuery(Guid EventId) : IQuery<IReadOnlyCollection<TicketTypeViewModel>>;
