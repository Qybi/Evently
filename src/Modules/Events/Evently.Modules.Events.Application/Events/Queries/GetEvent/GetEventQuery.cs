using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Events.Queries.ViewModels;

namespace Evently.Modules.Events.Application.Events.Queries.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IQuery<EventViewModel>;
