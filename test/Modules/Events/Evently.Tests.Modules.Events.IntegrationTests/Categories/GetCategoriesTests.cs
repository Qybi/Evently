using AwesomeAssertions;
using Evently.Modules.Events.Application.Categories.Queries.GetCategories;
using Evently.Modules.Events.Application.Categories.Queries.ViewModels;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Events.IntegrationTests.Categories;

public class GetCategoriesTests : BaseIntegrationTest
{
    public GetCategoriesTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnEmptyCollection_WhenNoCategoriesExist()
    {
        // Arrange
        await CleanDatabaseAsync();

        var query = new GetCategoriesQuery();

        // Act
        Result<IReadOnlyCollection<CategoryViewModel>> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnCategory_WhenCategoryExists()
    {
        // Arrange
        await CleanDatabaseAsync();

        await Sender.CreateCategoryAsync(Faker.Music.Genre());
        await Sender.CreateCategoryAsync(Faker.Music.Genre());

        var query = new GetCategoriesQuery();

        // Act
        Result<IReadOnlyCollection<CategoryViewModel>> result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }
}
