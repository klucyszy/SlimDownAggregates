using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.LibraryMembership;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

internal sealed class LibraryMembershipRepository : IAggregateRepository<LibraryMembershipAggregate>
{
    public Task<LibraryMembershipAggregate> GetAggregateAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(LibraryMembershipAggregate aggregate, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}