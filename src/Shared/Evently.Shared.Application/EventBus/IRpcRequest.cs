namespace Evently.Shared.Application.EventBus;

// Request sent over the bus where the caller waits for a reply (RPC). Binding the reply type to the request
// lets the caller infer it, so it always awaits the exact type the handler returns
public interface IRpcRequest<TReply>;
