using AwesomeAssertions;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using Evently.Tests.Modules.Users.UnitTests.Abstractions;

namespace Evently.Tests.Modules.Users.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnUser()
    {
        //Act
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        //Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenUserCreated()
    {
        //Act
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        //Assert
        UserRegisteredDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserRegisteredDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
    }

    [Fact]
    public void Create_ShouldAddMemberRoleToUser_WhenUserCreated()
    {
        //Act
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        //Assert
        user.Roles.Single().Should().Be(Role.Member);
    }

    [Fact]
    public void Update_ShouldRaiseDomainEvent_WhenUserUpdated()
    {
        //Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        string firstName = Faker.Name.FirstName();
        string lastName = Faker.Name.LastName();

        //Act
        user.Update(firstName, lastName);

        //Assert
        UserProfileUpdatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserProfileUpdatedDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
        domainEvent.FirstName.Should().Be(firstName);
        domainEvent.LastName.Should().Be(lastName);
    }

    [Fact]
    public void Update_ShouldNotRaiseDomainEvent_WhenUserNotUpdated()
    {
        //Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        user.ClearDomainEvents();

        //Act
        user.Update(user.FirstName, user.LastName);

        //Assert
        user.DomainEvents.Should().BeEmpty();
    }
}
