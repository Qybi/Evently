using Evently.Shared.Domain.DomainEvents;
using MediatR;

namespace Evently.Shared.Application.Messaging;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent;
