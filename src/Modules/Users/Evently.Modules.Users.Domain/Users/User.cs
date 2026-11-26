using System.Collections.ObjectModel;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using Evently.Modules.Users.Domain.Users.Errors;
using Evently.Shared.Domain;

namespace Evently.Modules.Users.Domain.Users;

public sealed class User : Entity
{
    private User() { }

    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string IdentityId { get; private set; }
    private readonly List<Permission> _permissions = [];
    public IReadOnlyCollection<Permission> Permissions => new ReadOnlyCollection<Permission>(_permissions);

    public static User Create(string email, string firstName, string lastName, string identityId)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            IdentityId = identityId
        };

        user._permissions.AddRange(Permission.DefaultForNewUser);

        user.Raise(new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    public void Update(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return;
        }

        FirstName = firstName;
        LastName = lastName;

        Raise(new UserProfileUpdatedDomainEvent(Id, FirstName, LastName));
    }

    public Result GrantPermission(Permission permission)
    {
        if (_permissions.Any(p => p.Code == permission.Code))
        {
            return Result.Failure(UserErrors.PermissionAlreadyGranted(Id, permission.Code));
        }

        _permissions.Add(permission);

        Raise(new UserPermissionGrantedDomainEvent(Id, permission.Code));

        return Result.Success();
    }

    public Result RevokePermission(Permission permission)
    {
        // Compare by Code: a Permission loaded by EF is not the same instance as the static one
        Permission? granted = _permissions.SingleOrDefault(p => p.Code == permission.Code);

        if (granted is null)
        {
            return Result.Failure(UserErrors.PermissionNotGranted(Id, permission.Code));
        }

        _permissions.Remove(granted);

        Raise(new UserPermissionRevokedDomainEvent(Id, permission.Code));

        return Result.Success();
    }
}
