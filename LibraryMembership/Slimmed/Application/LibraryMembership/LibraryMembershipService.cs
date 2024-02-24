using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Shared.Infrastructure.Abstractions;

namespace LibraryMembership.Slimmed.Application.LibraryMembership;

public interface ILibraryMembershipService
{
    Task<Result> LoanBookAsync(Guid membershipId, Guid bookId, CancellationToken ct);
    Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId, CancellationToken ct);
    Task<Result> ReserveBookAsync(Guid membershipId, Guid bookId, CancellationToken ct);
    Task<Result> CancelBookReservationAsync(Guid membershipId, Guid bookId, CancellationToken ct);
}

public sealed class LibraryMembershipService : ILibraryMembershipService
{
    private readonly IAggregateRepository<LibraryCardAggregate> _libraryCardAggregateRepository;

    public LibraryCardService(IAggregateRepository<LibraryCardAggregate> libraryCardAggregateRepository)
    {
        _libraryCardAggregateRepository = libraryCardAggregateRepository;
    }

    public async Task<Result> EvaluateMembershipStatus(Guid membershipId, CancellationToken ct)
    {
        // creating aggregate itself evaluates it's state
        LibraryCardAggregate? aggregate = await _libraryCardAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Membership not found");
        }

        await _libraryCardAggregateRepository.UpdateAsync(aggregate, ct);
        
        return Result.Success();
    }
}