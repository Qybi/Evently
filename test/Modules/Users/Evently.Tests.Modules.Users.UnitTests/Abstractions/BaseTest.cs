using Bogus;
using Evently.Shared.Domain;
using Evently.Shared.Domain.DomainEvents;

namespace Evently.Tests.Modules.Users.UnitTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Faker Faker = new();

    public static T AssertDomainEventWasPublished<T>(Entity entity) where T : IDomainEvent
    {
        T? domainEvent = entity.DomainEvents.OfType<T>().SingleOrDefault();

        if (domainEvent is null)
        {
            throw new Exception($"{typeof(T).Name} was not published");
        }

        return domainEvent;
    }
}
