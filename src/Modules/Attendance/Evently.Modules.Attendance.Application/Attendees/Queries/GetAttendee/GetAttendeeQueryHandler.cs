using Evently.Modules.Attendance.Application.Attendees.Queries.ViewModels;
using Evently.Modules.Attendance.Domain.Attendees.Errors;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;

namespace Evently.Modules.Attendance.Application.Attendees.Queries.GetAttendee;

internal sealed class GetAttendeeQueryHandler(IAttendeeQueries attendeeQueries)
    : IQueryHandler<GetAttendeeQuery, AttendeeViewModel>
{
    public async Task<Result<AttendeeViewModel>> Handle(GetAttendeeQuery request, CancellationToken cancellationToken)
    {
        AttendeeViewModel? attendee = await attendeeQueries.GetAsync(request.AttendeeId, cancellationToken);

        if (attendee is null)
        {
            return Result.Failure<AttendeeViewModel>(AttendeeErrors.NotFound(request.AttendeeId));
        }

        return attendee;
    }
}
