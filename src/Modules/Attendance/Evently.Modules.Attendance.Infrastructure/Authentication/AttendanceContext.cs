using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Shared.Application.Exceptions;
using Evently.Shared.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;

namespace Evently.Modules.Attendance.Infrastructure.Authentication;

internal sealed class AttendanceContext(IHttpContextAccessor httpContextAccessor) : IAttendanceContext
{
    public Guid AttendeeId => httpContextAccessor.HttpContext?.User.GetUserId() ??
                              throw new EventlyException("User identifier is unavailable");
}
