using Evently.Shared.Application.EventBus;
using Wolverine;

namespace Evently.Shared.Infrastructure.EventBus;

public static class RpcExtensions
{
    // Sends the request and waits for its reply. TReply is inferred from the request,
    // because Wolverine only matches a remote reply against the exact type the caller awaits
    public static Task<TReply> CallAsync<TReply>(
        this IMessageBus bus,
        IRpcRequest<TReply> request,
        CancellationToken cancellationToken = default)
    {
        return bus.InvokeAsync<TReply>(request, cancellationToken);
    }
}
