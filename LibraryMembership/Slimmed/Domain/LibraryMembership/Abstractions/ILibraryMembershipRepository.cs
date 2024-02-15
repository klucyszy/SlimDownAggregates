using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership.Abstractions;

public interface ILibraryMembershipRepository
{
    Task<LibraryMembershipAggregate?> GetAggregateAsync(Guid membershipId, CancellationToken ct);
    Task UpdateAsync(LibraryMembershipAggregate aggregate, CancellationToken ct);
}