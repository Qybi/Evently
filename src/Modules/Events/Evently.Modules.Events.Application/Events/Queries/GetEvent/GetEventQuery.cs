using Evently.Modules.Events.Application.Events.Queries.ViewModels;
using Evently.Shared.Application.Messaging;

namespace Evently.Modules.Events.Application.Events.Queries.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IQuery<EventViewModel>;
