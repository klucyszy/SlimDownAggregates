using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Shared.Infrastructure.Abstractions;
using LibraryMembership.Slimmed.Domain.LibraryCart;

namespace LibraryMembership.Slimmed.Application.LibraryCart;

public interface ILibraryCartService
{
    Task<Result> LoanBookAsync(Guid membershipId, Guid bookId, CancellationToken ct);
    Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId, CancellationToken ct);
    Task<Result> ProlongBookLoanAsync(Guid membershipId, Guid bookId, CancellationToken ct);
}

public sealed class LibraryCartService : ILibraryCartService
{
    private readonly IAggregateRepository<LibraryCartAggregate> _libraryCartAggregateRepository;

    public LibraryCartService(IAggregateRepository<LibraryCartAggregate> libraryCartAggregateRepository)
    {
        _libraryCartAggregateRepository = libraryCartAggregateRepository;
    }

    public Task<Result> LoanBookAsync(Guid membershipId, Guid bookId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId, CancellationToken ct)
    {
        LibraryCartAggregate? aggregate = await _libraryCartAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Library cart not found");
        }
        
        aggregate.Return(bookId);
        
        await _libraryCartAggregateRepository.UpdateAsync(aggregate, ct);
        
        return Result.Success();
    }

    public Task<Result> ProlongBookLoanAsync(Guid membershipId, Guid bookId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}