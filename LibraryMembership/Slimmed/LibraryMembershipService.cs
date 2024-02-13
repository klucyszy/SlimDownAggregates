using LibraryMembership.Database.Repositories;
using LibraryMembership.Slimmed;
using System;
using System.Threading.Tasks;
using LibraryMembership.Database;

namespace LibraryMembership.Services;

public interface ILibraryMembershipService
{
    Task<Result> AddLoanAsync(Guid membershipId, Guid bookId);
}

public sealed class LibraryMembershipService : ILibraryMembershipService
{
    private readonly LibraryMembershipRepository _libraryMembershipRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LibraryMembershipService(LibraryMembershipRepository libraryMembershipRepository,
        IUnitOfWork unitOfWork)
    {
        _libraryMembershipRepository = libraryMembershipRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> AddLoanAsync(Guid membershipId, Guid bookId)
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
            await _unitOfWork.SaveChangesAsync();
            
            return Result.Failure("Membership is not active");
        }

        activeMembership.LoanBook(new BookLoanModel(
            Guid.NewGuid(),
            bookId,
            DateTimeOffset.Now.AddDays(-1)));
        
        _libraryMembershipRepository.UpdateAsync(activeMembership);
        
        await _unitOfWork.SaveChangesAsync();
        
        return Result.Success();
    }
}