using Evently.Shared.Application.Authorization;
using Evently.Shared.Application.EventBus;
using Evently.Shared.Domain.Errors;

namespace Evently.Modules.Users.IntegrationEvents;

public sealed record GetUserPermissionsReply(PermissionsResponse? Value, Error? Error) : RpcReply<PermissionsResponse>(Value, Error);
