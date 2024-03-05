using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.BookLoan;

namespace LibraryMembership.Slimmed.Application.LibraryCart;

public interface ILibraryCartService
{
    Task<Result<Guid>> LoanBookAsync(Guid membershipId, string bookIsbn, CancellationToken ct);
    Task<Result> ReturnBookAsync(Guid membershipId, Guid loanId, CancellationToken ct);
    Task<Result> ProlongBookLoanAsync(Guid membershipId, Guid loanId, CancellationToken ct);
}

public sealed class LibraryCartService : ILibraryCartService
{
    private readonly IAggregateRepository<Domain.LibraryCart.LibraryCartAggregate> _libraryCartAggregateRepository;
    private readonly IAggregateRepository<BookLoan> _bookLoanAggregateRepository;

    public LibraryCartService(IAggregateRepository<Domain.LibraryCart.LibraryCartAggregate> libraryCartAggregateRepository,
        IAggregateRepository<BookLoan> bookLoanAggregateRepository)
    {
        _libraryCartAggregateRepository = libraryCartAggregateRepository;
        _bookLoanAggregateRepository = bookLoanAggregateRepository;
    }

    public async Task<Result<Guid>> LoanBookAsync(Guid membershipId, string bookIsbn, CancellationToken ct)
    {
        // load book to check it's isbn
        var bookId = Guid.NewGuid();
        
        Domain.LibraryCart.LibraryCartAggregate aggregate = await _libraryCartAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result<Guid>.Failure("Library cart not found");
        }
        
        BookLoan result = aggregate.Loan(bookId, bookIsbn);
        
        await _libraryCartAggregateRepository.UpdateAsync(aggregate, true, ct);
        
        return Result<Guid>.Success(result.Id);
    }

    public async Task<Result> ReturnBookAsync(Guid membershipId, Guid loanId, CancellationToken ct)
    {
        BookLoan aggregate = await _bookLoanAggregateRepository
            .GetAggregateAsync(loanId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Loan not found");
        }
        
        aggregate.Return();
        
        await _bookLoanAggregateRepository.UpdateAsync(aggregate, true, ct);
        
        return Result.Success();
    }

    public async Task<Result> ProlongBookLoanAsync(Guid membershipId, Guid loanId, CancellationToken ct)
    {
        BookLoan aggregate = await _bookLoanAggregateRepository
            .GetAggregateAsync(loanId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Loan not found");
        }
        
        aggregate.Prolong();
        
        await _bookLoanAggregateRepository.UpdateAsync(aggregate, true, ct);
        
        return Result.Success();
    }
}