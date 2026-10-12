using Evently.Shared.Domain;
using Evently.Shared.Domain.Errors;

namespace Evently.Shared.Application.EventBus;

// Reply to an IRpcRequest. A request/reply can only return one reply type (in Wolverine), so success and failure travel in the same wrapper.
// Concrete replies inherit it with their own TValue: the concrete type is what get registered with Wolverine. This abstracts away the shape of the reply, and the concrete reply only needs to set
// the TValue type
public abstract record RpcReply<TValue>
    where TValue : class
{
    protected RpcReply(TValue? value, Error? error)
    {
        if (value is null == error is null)
        {
            throw new ArgumentException("Exactly one of value or error must be set.");
        }

        Value = value;
        Error = error;
    }

    public TValue? Value { get; }

    public Error? Error { get; }

    public Result<TValue> ToResult() =>
        Error is null
            ? Result.Success(Value!)
            : Result.Failure<TValue>(Error);
}
