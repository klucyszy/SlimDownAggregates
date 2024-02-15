using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Abstractions;

namespace LibraryMembership.Slimmed.Application.LibraryMembership;

public interface ILibraryMembershipService
{
    Task<Result> LoanBookAsync(Guid membershipId, Guid bookId, CancellationToken ct);
    Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId, CancellationToken ct);
}

public sealed class LibraryMembershipService : ILibraryMembershipService
{
    private readonly ILibraryMembershipRepository _libraryMembershipRepository;

    public LibraryMembershipService(ILibraryMembershipRepository libraryMembershipRepository)
    {
        _libraryMembershipRepository = libraryMembershipRepository;
    }

    public async Task<Result> LoanBookAsync(Guid membershipId, Guid bookId, CancellationToken ct)
    {
        LibraryMembershipAggregate? aggregate = await _libraryMembershipRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Membership not found");
        }
        
        if (aggregate is not LibraryMembershipAggregate.Active activeMembership)
        {
            // Persist aggregate state if it was changed
            await _libraryMembershipRepository.UpdateAsync(aggregate, ct);
            
            return Result.Failure("Membership is not active");
        }

        activeMembership.LoanBook(new BookLoan(
            Guid.NewGuid(),
            bookId,
            DateTimeOffset.Now.AddDays(30)));
        
        await _libraryMembershipRepository.UpdateAsync(activeMembership, ct);
        
        return Result.Success();
    }

    public async Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId, CancellationToken ct)
    {
        LibraryMembershipAggregate? aggregate = await _libraryMembershipRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Membership not found");
        }
        
        aggregate.ReturnBook(bookId);
        
        await _libraryMembershipRepository.UpdateAsync(aggregate, ct);
        
        return Result.Success();
    }
}