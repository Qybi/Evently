using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.TicketTypes.Queries.ViewModels;

namespace Evently.Modules.Events.Application.TicketTypes.Queries.GetEventTicketTypes;

public sealed record GetEventTicketTypesQuery(Guid EventId) : IQuery<IReadOnlyCollection<TicketTypeViewModel>>;
