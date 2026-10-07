using AwesomeAssertions;
using Evently.Modules.Ticketing.Application.Customers.Queries.GetCustomer;
using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Ticketing.IntegrationTests.Customers;

public class GetCustomerByIdTests : BaseIntegrationTest
{
    public GetCustomerByIdTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        var query = new GetCustomerByIdQuery(Guid.NewGuid());

        // Act
        Result result = await SendQuery<GetCustomerByIdQuery, CustomerViewModel>(query);

        // Assert
        result.Error.Should().Be(CustomerErrors.NotFound(query.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        Guid customerId = await CreateCustomerAsync(Guid.NewGuid());

        var query = new GetCustomerByIdQuery(customerId);

        // Act
        Result<CustomerViewModel> result = await SendQuery<GetCustomerByIdQuery, CustomerViewModel>(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
}