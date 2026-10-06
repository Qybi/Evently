using AwesomeAssertions;
using Evently.Modules.Attendance.Application.Attendees.Queries.GetAttendee;
using Evently.Modules.Attendance.Application.Attendees.Queries.ViewModels;
using Evently.Modules.Ticketing.Application.Customers.Queries.GetCustomer;
using Evently.Modules.Ticketing.Application.Customers.Queries.ViewModels;
using Evently.Modules.Users.Application.Users.Commands.RegisterUser;
using Evently.Shared.Domain;
using Evently.Tests.IntegrationTests.Abstractions;

namespace Evently.Tests.IntegrationTests.RegisterUser;

public class RegisterUserTests : BaseIntegrationTest
{
    public RegisterUserTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToTicketingModule()
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
            TimeSpan.FromSeconds(35),
            async () =>
            {
                var query = new GetCustomerByIdQuery(userResult.Value);

                Result<CustomerViewModel> customerResult = await TicketingSender.Send(query);

                return customerResult;
            });

        // Assert
        customerResult.IsSuccess.Should().BeTrue();
        customerResult.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToAttendanceModule()
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
        Result<AttendeeViewModel> attendeeResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(35),
            async () =>
            {
                var query = new GetAttendeeQuery(userResult.Value);

                Result<AttendeeViewModel> attendeeResult = await Sender.Send(query);

                return attendeeResult;
            });

        // Assert
        attendeeResult.IsSuccess.Should().BeTrue();
        attendeeResult.Value.Should().NotBeNull();
    }
}
