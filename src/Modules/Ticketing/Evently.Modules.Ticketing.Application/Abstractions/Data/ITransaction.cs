namespace Evently.Modules.Ticketing.Application.Abstractions.Data;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
