using Evently.Shared.Domain.Errors;

namespace Evently.Shared.Application.Exceptions;

public sealed class EventlyException : Exception
{
    public EventlyException(string requestName, Error? error = default, Exception? innerException = default) : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }
    public Error? Error { get; }
}
