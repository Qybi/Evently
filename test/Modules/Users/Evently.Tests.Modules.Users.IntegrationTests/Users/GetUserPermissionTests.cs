using AwesomeAssertions;
using Evently.Modules.Users.Application.Users.Commands.RegisterUser;
using Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;
using Evently.Modules.Users.Domain.Users.Errors;
using Evently.Shared.Application.Authorization;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Users.IntegrationTests.Abstraction;

namespace Evently.Tests.Modules.Users.IntegrationTests.Users;

public class GetUserPermissionTests : BaseIntegrationTest
{
    public GetUserPermissionTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        string identityId = Guid.NewGuid().ToString();

        // Act
        Result<PermissionsResponse> permissionsResult = await SendQuery<GetUserPermissionsQuery, PermissionsResponse>(new GetUserPermissionsQuery(identityId));

        // Assert
        permissionsResult.Error.Should().Be(UserErrors.NotFound(identityId));
    }

    [Fact]
    public async Task Should_ReturnPermissions_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await SendCommand<RegisterUserCommand, Guid>(new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            Faker.Name.FirstName(),
            Faker.Name.LastName()));

        string identityId = DbContext.Users.Single(u => u.Id == result.Value).IdentityId;

        // Act
        Result<PermissionsResponse> permissionsResult = await SendQuery<GetUserPermissionsQuery, PermissionsResponse>(new GetUserPermissionsQuery(identityId));

        // Assert
        permissionsResult.IsSuccess.Should().BeTrue();
        permissionsResult.Value.Permissions.Should().NotBeEmpty();
    }
}
