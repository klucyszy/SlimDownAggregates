using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.LibraryMembership;

namespace LibraryMembership.Slimmed.Application.LibraryMembership;

public interface ILibraryMembershipService
{
}

public sealed class LibraryMembershipService : ILibraryMembershipService
{
    private readonly IAggregateRepository<Domain.LibraryMembership.LibraryMembershipAggregate> _libraryMembershipAggregateRepository;

    public LibraryMembershipService(IAggregateRepository<Domain.LibraryMembership.LibraryMembershipAggregate> libraryMembershipAggregateRepository)
    {
        _libraryMembershipAggregateRepository = libraryMembershipAggregateRepository;
    }

    public async Task<Result> EvaluateMembershipStatus(Guid membershipId, CancellationToken ct)
    {
        // creating aggregate itself evaluates it's state
        LibraryMembershipAggregate aggregate = await _libraryMembershipAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Membership not found");
        }

        await _libraryMembershipAggregateRepository.UpdateAsync(aggregate, ct);
        
        return Result.Success();
    }
}