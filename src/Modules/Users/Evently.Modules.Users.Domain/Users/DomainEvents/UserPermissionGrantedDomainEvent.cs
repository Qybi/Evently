using Evently.Shared.Domain.DomainEvents;

namespace Evently.Modules.Users.Domain.Users.DomainEvents;

public sealed class UserPermissionGrantedDomainEvent(Guid userId, string permission) : DomainEvent
{
    public Guid UserId { get; init; } = userId;

    public string Permission { get; init; } = permission;
}
