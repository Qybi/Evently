using AwesomeAssertions;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Application.Carts.AddItemToCart;
using Evently.Modules.Ticketing.Application.Orders.Commands.CreateOrder;
using Evently.Modules.Ticketing.Application.Orders.Queries.GetOrders;
using Evently.Modules.Ticketing.Application.Orders.ViewModels;
using Evently.Modules.Ticketing.Application.Tickets.Queries.GetTicketForOrder;
using Evently.Modules.Ticketing.Application.Tickets.Queries.ViewModels;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Shared.Domain;
using Evently.Tests.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Tests.Modules.Ticketing.IntegrationTests.Orders;

public class CreateOrderTests : BaseIntegrationTest
{
    private const decimal Quantity = 2;

    public CreateOrderTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        //Arrange
        var command = new CreateOrderCommand(Guid.NewGuid());

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(CustomerErrors.NotFound(command.CustomerId));
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCartIsEmpty()
    {
        //Arrange
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());

        var command = new CreateOrderCommand(customerId);

        //Act
        Result result = await Sender.Send(command);

        //Assert
        result.Error.Should().Be(CartErrors.Empty);
    }

    [Fact]
    public async Task Should_IssueTickets_WhenOrderIsCreated()
    {
        //Arrange
        Guid customerId = await Sender.CreateCustomerAsync(Guid.NewGuid());
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();

        await Sender.CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity);
        await Sender.Send(new AddItemToCartCommand(customerId, ticketTypeId, Quantity));

        //Act
        Result result = await Sender.Send(new CreateOrderCommand(customerId));

        //Assert
        result.IsSuccess.Should().BeTrue();

        Result<IReadOnlyCollection<GetOrdersViewModel>> ordersResult = await Sender.Send(new GetOrdersQuery(customerId));
        Guid orderId = ordersResult.Value.Single().Id;

        // Tickets are issued asynchronously by the outbox job.
        IReadOnlyCollection<TicketViewModel> tickets = [];
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        DateTime endTimeUtc = DateTime.UtcNow.AddSeconds(40);

        while (tickets.Count == 0 && DateTime.UtcNow < endTimeUtc && await timer.WaitForNextTickAsync())
        {
            Result<IReadOnlyCollection<TicketViewModel>> ticketsResult = await Sender.Send(new GetTicketsForOrderQuery(orderId));
            tickets = ticketsResult.Value;
        }

        tickets.Should().HaveCount((int)Quantity);
    }
}
