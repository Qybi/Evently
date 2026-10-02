using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Evently.Modules.Ticketing.Infrastructure.Database;

internal sealed class EfTransaction(IDbContextTransaction transaction) : ITransaction
{
    public Task CommitAsync(CancellationToken cancellationToken = default) =>
        transaction.CommitAsync(cancellationToken);

    public ValueTask DisposeAsync() => transaction.DisposeAsync();
}
