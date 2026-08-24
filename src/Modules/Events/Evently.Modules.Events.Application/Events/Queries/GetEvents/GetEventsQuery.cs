using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.Queries.GetEvents;

public sealed record GetEventsQuery : IQuery<IReadOnlyCollection<EventViewModel>>;
