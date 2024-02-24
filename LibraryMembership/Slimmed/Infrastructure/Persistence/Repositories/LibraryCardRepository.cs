using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Infrastructure.Abstractions;
using LibraryMembership.Slimmed.Domain.LibraryCart;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

internal sealed class LibraryCardRepository : IAggregateRepository<LibraryCartAggregate>
{
    public Task<LibraryCartAggregate?> GetAggregateAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(LibraryCartAggregate aggregate, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}