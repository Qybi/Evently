using AwesomeAssertions;
using Evently.Modules.Ticketing.Application.Carts.AddItemToCart;
using Evently.Modules.Ticketing.Application.Customers.Queries.GetCustomer;
using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;
using Evently.Modules.Users.Application.Users.Commands.RegisterUser;
using Evently.Shared.Domain;
using Evently.Tests.IntegrationTests.Abstractions;

namespace Evently.Tests.IntegrationTests.AddToCart;

public sealed class AddItemToCartTests : BaseIntegrationTest
{
    private const decimal Quantity = 10;

    public AddItemToCartTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Customer_ShouldBeAbleTo_AddItemToCart()
    {
        // Register user
        var command = new RegisterUserCommand(
            Faker.Internet.Email(),
            Faker.Internet.Password(6),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Result<Guid> userResult = await Sender.Send(command);

        userResult.IsSuccess.Should().BeTrue();

        // Get customer
        Result<CustomerViewModel> customerResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(40),
            async () =>
            {
                var query = new GetCustomerByIdQuery(userResult.Value);

                Result<CustomerViewModel> customerResult = await Sender.Send(query);

                return customerResult;
            });

        customerResult.IsSuccess.Should().BeTrue();

        // Add item to cart
        CustomerViewModel customer = customerResult.Value;
        var ticketTypeId = Guid.NewGuid();

        await Sender.CreateEventAsync(Guid.NewGuid(), ticketTypeId, Quantity);

        Result result = await Sender.Send(new AddItemToCartCommand(customer.Id, ticketTypeId, Quantity));

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
