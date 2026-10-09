using AwesomeAssertions;
using Evently.Modules.Users.Application.Users.Commands.RegisterUser;
using Evently.Modules.Users.Application.Users.Queries.GetUser;
using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Modules.Users.Domain.Users.Errors;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Users.IntegrationTests.Abstraction;

namespace Evently.Tests.Modules.Users.IntegrationTests.Users;

public class GetUserTests : BaseIntegrationTest
{
    public GetUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        Result<UserViewModel> userResult = await SendQuery<GetUserQuery, UserViewModel>(new GetUserQuery(userId));

        // Assert
        userResult.Error.Should().Be(UserErrors.NotFound(userId));
    }

    [Fact]
    public async Task Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await SendCommand<RegisterUserCommand, Guid>(new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(),
            Faker.Name.FirstName(),
            Faker.Name.LastName()));
        Guid userId = result.Value;

        // Act
        Result<UserViewModel> userResult = await SendQuery<GetUserQuery, UserViewModel>(new GetUserQuery(userId));

        // Assert
        userResult.IsSuccess.Should().BeTrue();
        userResult.Value.Should().NotBeNull();
    }
}
