using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.BookLoan;

namespace LibraryMembership.Slimmed.Application.LibraryCart;

public interface ILibraryCartService
{
    Task<Result> LoanBookAsync(Guid membershipId, string bookIsbn, CancellationToken ct);
    Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId, CancellationToken ct);
    Task<Result> ProlongBookLoanAsync(Guid membershipId, Guid bookId, CancellationToken ct);
}

public sealed class LibraryCartService : ILibraryCartService
{
    private readonly IAggregateRepository<Domain.LibraryCart.LibraryCart> _libraryCartAggregateRepository;
    private readonly IAggregateRepository<BookLoan> _bookLoanAggregateRepository;

    public LibraryCartService(IAggregateRepository<Domain.LibraryCart.LibraryCart> libraryCartAggregateRepository,
        IAggregateRepository<BookLoan> bookLoanAggregateRepository)
    {
        _libraryCartAggregateRepository = libraryCartAggregateRepository;
        _bookLoanAggregateRepository = bookLoanAggregateRepository;
    }

    public async Task<Result> LoanBookAsync(Guid membershipId, string bookIsbn, CancellationToken ct)
    {
        // load book to check it's isbn
        var bookId = Guid.NewGuid();
        
        Domain.LibraryCart.LibraryCart aggregate = await _libraryCartAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Library cart not found");
        }
        
        aggregate.Loan(membershipId, bookId, bookIsbn);
        
        await _libraryCartAggregateRepository.UpdateAsync(aggregate, true, ct);
        
        return Result.Success();
    }

    public async Task<Result> ReturnBookAsync(Guid membershipId, Guid bookId, CancellationToken ct)
    {
        BookLoan aggregate = await _bookLoanAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Loan not found");
        }
        
        aggregate.Return();
        
        await _bookLoanAggregateRepository.UpdateAsync(aggregate, true, ct);
        
        return Result.Success();
    }

    public async Task<Result> ProlongBookLoanAsync(Guid membershipId, Guid bookId, CancellationToken ct)
    {
        BookLoan aggregate = await _bookLoanAggregateRepository
            .GetAggregateAsync(membershipId, ct);
        
        if (aggregate is null)
        {
            return Result.Failure("Loan not found");
        }
        
        aggregate.Return();
        
        await _bookLoanAggregateRepository.UpdateAsync(aggregate, true, ct);
        
        return Result.Success();
    }
}