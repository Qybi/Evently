namespace Evently.Modules.Users.PublicApi;

public sealed record UserPublicApiResponse(Guid Id, string Email, string FirstName, string LastName);
