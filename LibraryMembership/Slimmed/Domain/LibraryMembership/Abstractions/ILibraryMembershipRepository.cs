using System;
using System.Threading.Tasks;
using LibraryMembership.Slimmed.Domain.LibraryMembership;

namespace LibraryMembership.Slimmed;

public interface ILibraryMembershipRepository
{
    Task<LibraryMembershipAggregate?> GetAggregateAsync(Guid membershipId);
    Task UpdateAsync(LibraryMembershipAggregate aggregate);
}