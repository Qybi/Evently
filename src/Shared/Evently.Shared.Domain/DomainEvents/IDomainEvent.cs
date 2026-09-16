using MediatR;

namespace Evently.Shared.Domain.DomainEvents;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredOnUtc { get; }
}
