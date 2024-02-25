using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Shared.Infrastructure.Abstractions;
using LibraryMembership.Slimmed.Domain.LibraryMembership;

namespace LibraryMembership.Slimmed.Application.LibraryMembership;

public interface ILibraryMembershipService
{
}

public sealed class LibraryMembershipService : ILibraryMembershipService
{
    private readonly IAggregateRepository<LibraryMembershipAggregate> _libraryMembershipAggregateRepository;

    public LibraryMembershipService(IAggregateRepository<LibraryMembershipAggregate> libraryMembershipAggregateRepository)
    {
        _libraryMembershipAggregateRepository = libraryMembershipAggregateRepository;
    }

    public async Task<Result> EvaluateMembershipStatus(Guid membershipId, CancellationToken ct)
    {
        // creating aggregate itself evaluates it's state
        LibraryMembershipAggregate? aggregate = await _libraryMembershipAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Membership not found");
        }

        await _libraryMembershipAggregateRepository.UpdateAsync(aggregate, ct);
        
        return Result.Success();
    }
}