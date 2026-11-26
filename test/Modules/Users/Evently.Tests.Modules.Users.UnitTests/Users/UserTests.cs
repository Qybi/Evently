using AwesomeAssertions;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using Evently.Modules.Users.Domain.Users.Errors;
using Evently.Shared.Domain;
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
    public void Create_ShouldGrantDefaultPermissionsToUser_WhenUserCreated()
    {
        //Act
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        //Assert
        user.Permissions.Should().BeEquivalentTo(Permission.DefaultForNewUser);
    }

    [Fact]
    public void GrantPermission_ShouldAddPermission_WhenNotGranted()
    {
        //Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        //Act
        Result result = user.GrantPermission(Permission.GetEventStatistics);

        //Assert
        result.IsSuccess.Should().BeTrue();
        user.Permissions.Should().ContainSingle(p => p.Code == Permission.GetEventStatistics.Code);
    }

    [Fact]
    public void GrantPermission_ShouldRaiseDomainEvent_WhenNotGranted()
    {
        //Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        //Act
        user.GrantPermission(Permission.GetEventStatistics);

        //Assert
        UserPermissionGrantedDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserPermissionGrantedDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
        domainEvent.Permission.Should().Be(Permission.GetEventStatistics.Code);
    }

    [Fact]
    public void GrantPermission_ShouldReturnFailure_WhenAlreadyGranted()
    {
        //Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        // Same code as a default permission, different instance: mimics a Permission loaded by EF
        var permission = new Permission(Permission.GetUser.Code);

        //Act
        Result result = user.GrantPermission(permission);

        //Assert
        result.Error.Should().Be(UserErrors.PermissionAlreadyGranted(user.Id, permission.Code));
    }

    [Fact]
    public void RevokePermission_ShouldRemovePermission_WhenGranted()
    {
        //Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        var permission = new Permission(Permission.GetUser.Code);

        //Act
        Result result = user.RevokePermission(permission);

        //Assert
        result.IsSuccess.Should().BeTrue();
        user.Permissions.Should().NotContain(p => p.Code == permission.Code);
    }

    [Fact]
    public void RevokePermission_ShouldRaiseDomainEvent_WhenGranted()
    {
        //Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        //Act
        user.RevokePermission(Permission.GetUser);

        //Assert
        UserPermissionRevokedDomainEvent domainEvent =
            AssertDomainEventWasPublished<UserPermissionRevokedDomainEvent>(user);

        domainEvent.UserId.Should().Be(user.Id);
        domainEvent.Permission.Should().Be(Permission.GetUser.Code);
    }

    [Fact]
    public void RevokePermission_ShouldReturnFailure_WhenNotGranted()
    {
        //Arrange
        var user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid().ToString());

        //Act
        Result result = user.RevokePermission(Permission.GetEventStatistics);

        //Assert
        result.Error.Should().Be(UserErrors.PermissionNotGranted(user.Id, Permission.GetEventStatistics.Code));
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
