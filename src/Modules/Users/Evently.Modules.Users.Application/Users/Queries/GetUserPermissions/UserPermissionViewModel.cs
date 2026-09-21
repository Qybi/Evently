namespace Evently.Modules.Users.Application.Users.Queries.GetUserPermissions;

public sealed record UserPermissionViewModel
{
    public Guid UserId { get; init; }
    public string Permission { get; set; }
}
