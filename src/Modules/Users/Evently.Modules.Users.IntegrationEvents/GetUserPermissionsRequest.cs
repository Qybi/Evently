using Evently.Shared.Application.EventBus;

namespace Evently.Modules.Users.IntegrationEvents;

public sealed record GetUserPermissionsRequest(string IdentityId) : IRpcRequest<GetUserPermissionsReply>;

