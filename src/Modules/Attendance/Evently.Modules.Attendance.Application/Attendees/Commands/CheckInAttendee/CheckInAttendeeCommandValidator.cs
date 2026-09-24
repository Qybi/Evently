using FluentValidation;

namespace Evently.Modules.Attendance.Application.Attendees.Commands.CheckInAttendee;

internal sealed class CheckInAttendeeCommandValidator : AbstractValidator<CheckInAttendeeCommand>
{
    public CheckInAttendeeCommandValidator()
    {
        RuleFor(c => c.AttendeeId).NotEmpty();
        RuleFor(c => c.TicketId).NotEmpty();
    }
}
