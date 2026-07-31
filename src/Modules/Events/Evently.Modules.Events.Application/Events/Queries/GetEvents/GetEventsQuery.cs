using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;

namespace Evently.Modules.Events.Application.Events.Queries.GetEvents;

public sealed record GetEventsQuery : IQuery<IReadOnlyCollection<EventViewModel>>;
