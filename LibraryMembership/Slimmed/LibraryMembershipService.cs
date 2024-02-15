using System;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Slimmed.Domain.LibraryMembership;

namespace LibraryMembership.Slimmed;

public interface ILibraryMembershipService
{
    Task<Result> LoanBookAsync(Guid membershipId, Guid bookId);
    Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId);
}

public sealed class LibraryMembershipService : ILibraryMembershipService
{
    private readonly LibraryMembershipRepository _libraryMembershipRepository;

    public LibraryMembershipService(LibraryMembershipRepository libraryMembershipRepository)
    {
        _libraryMembershipRepository = libraryMembershipRepository;
    }

    public async Task<Result> LoanBookAsync(Guid membershipId, Guid bookId)
    {
        LibraryMembershipAggregate? aggregate = await _libraryMembershipRepository
            .GetAggregateAsync(membershipId);
        
        if (aggregate is null)
        {
            return Result.Failure("Membership not found");
        }
        
        if (aggregate is not LibraryMembershipAggregate.Active activeMembership)
        {
            // Persist aggregate state if it was changed
            _libraryMembershipRepository.UpdateAsync(aggregate);
            
            return Result.Failure("Membership is not active");
        }

        activeMembership.LoanBook(new BookLoan(
            Guid.NewGuid(),
            bookId,
            DateTimeOffset.Now.AddDays(30)));
        
        _libraryMembershipRepository.UpdateAsync(activeMembership);
        
        return Result.Success();
    }

    public async Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId)
    {
        LibraryMembershipAggregate? aggregate = await _libraryMembershipRepository
            .GetAggregateAsync(membershipId);
        
        if (aggregate is null)
        {
            return Result.Failure("Membership not found");
        }
        
        aggregate.ReturnBook(bookId);
        
        _libraryMembershipRepository.UpdateAsync(aggregate);
        
        return Result.Success();
    }
}