using System.Collections.ObjectModel;
using Evently.Modules.Users.Domain.Users.DomainEvents;
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
    private readonly List<Role> _roles = [];
    public IReadOnlyCollection<Role> Roles => new ReadOnlyCollection<Role>(_roles);

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

        user._roles.Add(Role.Member);

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
}
