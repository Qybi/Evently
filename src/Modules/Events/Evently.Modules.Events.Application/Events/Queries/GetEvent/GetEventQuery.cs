using Evently.Modules.Events.Application.Events.DTO;
using MediatR;

namespace Evently.Modules.Events.Application.Events.Queries.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IRequest<EventResponseDto?>;
