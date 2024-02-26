using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryMembership.Shared.Domain.Abstractions;

public interface IAggregateRepository<TAggregate>
{
    Task<TAggregate> GetAggregateAsync(Guid id, CancellationToken ct = default);
    Task UpdateAsync(TAggregate aggregate, bool saveChanges = true, CancellationToken ct = default);
}