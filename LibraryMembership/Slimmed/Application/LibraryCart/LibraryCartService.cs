using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.BookLoan;
using LibraryMembership.Slimmed.Domain.LibraryCart;
using LibraryMembership.Slimmed.Domain.Shared;

namespace LibraryMembership.Slimmed.Application.LibraryCart;

public interface ILibraryCartService
{
    Task<Result<Guid>> LoanBookAsync(Guid membershipId, string bookIsbn, CancellationToken ct);
    Task<Result> ReturnBookAsync(Guid membershipId, Guid loanId, CancellationToken ct);
    Task<Result> ProlongBookLoanAsync(Guid membershipId, Guid loanId, CancellationToken ct);
}

public sealed class LibraryCartService : ILibraryCartService
{
    private readonly IAggregateRepository<LibraryCartAggregate> _libraryCartAggregateRepository;
    private readonly IAggregateRepository<BookLoanAggregate> _bookLoanAggregateRepository;
    private readonly IClock _clock;

    public LibraryCartService(IAggregateRepository<LibraryCartAggregate> libraryCartAggregateRepository,
        IAggregateRepository<BookLoanAggregate> bookLoanAggregateRepository, IClock clock)
    {
        _libraryCartAggregateRepository = libraryCartAggregateRepository;
        _bookLoanAggregateRepository = bookLoanAggregateRepository;
        _clock = clock;
    }

    public async Task<Result<Guid>> LoanBookAsync(Guid membershipId, string bookIsbn, CancellationToken ct)
    {
        // load book to check it's isbn
        var bookId = Guid.NewGuid();
        
        LibraryCartAggregate aggregate = await _libraryCartAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result<Guid>.Failure("Library cart not found");
        }
        
        aggregate.Loan(bookId, bookIsbn, _clock);
        
        await _libraryCartAggregateRepository.UpdateAsync(aggregate, ct);
        
        LibraryCartAggregate updated = await _libraryCartAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        return Result<Guid>.Success(Guid.NewGuid());
    }

    public async Task<Result> ReturnBookAsync(Guid membershipId, Guid loanId, CancellationToken ct)
    {
        BookLoanAggregate aggregate = await _bookLoanAggregateRepository
            .GetAggregateAsync(loanId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Loan not found");
        }
        
        aggregate.Return(_clock);
        
        await _bookLoanAggregateRepository.UpdateAsync(aggregate, ct);
        
        return Result.Success();
    }

    public async Task<Result> ProlongBookLoanAsync(Guid membershipId, Guid bookId, CancellationToken ct)
    {
        LibraryCartAggregate aggregate = await _libraryCartAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Loan not found");
        }
        
        aggregate.Prolong(bookId);
        
        await _libraryCartAggregateRepository.UpdateAsync(aggregate, ct);
        
        return Result.Success();
    }
}