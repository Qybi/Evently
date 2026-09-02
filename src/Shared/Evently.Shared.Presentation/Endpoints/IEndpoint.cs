using Microsoft.AspNetCore.Routing;

namespace Evently.Shared.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
