using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Domain.Abstractions;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

internal sealed class LibraryMembershipRepository : IAggregateRepository<Domain.LibraryMembership.LibraryMembership>
{
    public Task<Domain.LibraryMembership.LibraryMembership> GetAggregateAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Domain.LibraryMembership.LibraryMembership aggregate, bool saveChanges = true, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}