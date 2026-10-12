namespace Evently.Shared.Application.EventBus;

// Handler side of an RPC request: Wolverine calls Handle and sends the returned reply back to the caller.
// The constraint ties request and reply together, so a consumer replying with the wrong type does not compile
public interface IRpcConsumer<in TRequest, TReply>
    where TRequest : IRpcRequest<TReply>
{
    Task<TReply> Handle(TRequest request, CancellationToken cancellationToken);
}
