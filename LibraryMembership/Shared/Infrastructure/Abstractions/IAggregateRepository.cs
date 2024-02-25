using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryMembership.Shared.Infrastructure.Abstractions;

public interface IAggregateRepository<TAggregate>
{
    Task<TAggregate?> GetAggregateAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(TAggregate aggregate, CancellationToken ct);
}