using LibraryMembership.Database.Repositories;
using LibraryMembership.Slimmed;
using System;
using System.Threading.Tasks;

namespace LibraryMembership.Services;

public sealed class LibraryMembershipService
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
        LibraryMembershipModel? model = await _libraryMembershipRepository
            .GetAsync(membershipId);
        
        if (model is null)
        {
            return Result.Failure("Membership not found");
        }
        
        LibraryMembershipAggregate aggregate = model.ToAggregate(DateTimeOffset.Now);

        if (aggregate is not LibraryMembershipAggregate.Active activeMembership)
        {
            return Result.Failure("Membership is not active");
        }

        activeMembership.AddLoan(new BookLoan(Guid.NewGuid(), bookId, DateTimeOffset.Now));

        _libraryMembershipRepository.UpdateAsync(activeMembership.ToModel(model));
        
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}