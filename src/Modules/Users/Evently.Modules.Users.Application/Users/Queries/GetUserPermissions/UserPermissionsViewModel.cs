namespace Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;

public sealed record UserPermissionsViewModel
{
    public Guid UserId { get; init; }
    public List<string> Permissions { get; init; } = [];
}
