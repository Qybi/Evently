using Evently.Shared.Domain.Errors;

namespace Evently.Modules.Users.Domain.Users.Errors;

public static class UserErrors
{
    public static Error NotFound(Guid userId) =>
        Error.NotFound("Users.NotFound", $"The user with the identifier {userId} not found");

    public static Error NotFound(string identityId) =>
        Error.NotFound("Users.NotFound", $"The user with the IDP identifier {identityId} not found");

    public static Error PermissionAlreadyGranted(Guid userId, string permission) =>
        Error.Conflict("Users.PermissionAlreadyGranted", $"The user with the identifier {userId} already has the permission {permission}");

    public static Error PermissionNotGranted(Guid userId, string permission) =>
        Error.NotFound("Users.PermissionNotGranted", $"The user with the identifier {userId} does not have the permission {permission}");
}
