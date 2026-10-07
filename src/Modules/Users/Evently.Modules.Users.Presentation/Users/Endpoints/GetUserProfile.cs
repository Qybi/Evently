using System.Security.Claims;
using Evently.Modules.Users.Application.Users.Queries.GetUser;
using Evently.Modules.Users.Application.Users.Queries.ViewModels;
using Evently.Shared.Application.Messaging;
using Evently.Shared.Domain;
using Evently.Shared.Infrastructure.Authentication;
using Evently.Shared.Presentation.ApiResults;
using Evently.Shared.Presentation.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Users.Presentation.Users.Endpoints;

internal sealed class GetUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/profile", async (
            ClaimsPrincipal claims,
            IQueryHandler<GetUserQuery, UserViewModel> handler,
            CancellationToken cancellationToken) =>
        {
            Result<UserViewModel> result = await handler.Handle(new GetUserQuery(claims.GetUserId()), cancellationToken);

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization(Permissions.GetUser)
        .WithTags(Tags.Users);
    }

    internal sealed class Request
    {
        public string FirstName { get; init; }

        public string LastName { get; init; }
    }
}
