using System;
using System.Collections.Generic;
using System.Linq;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Shared.Domain.Exceptions;
using LibraryMembership.Slimmed.Domain.BookLoan;

namespace LibraryMembership.Slimmed.Domain.LibraryCart;

public sealed class LibraryCart : AggregateRoot
{
    private readonly Guid _membershipId;
    
    private readonly List<BookLoan.BookLoan> _activeBookLoans;
    public IReadOnlyList<BookLoan.BookLoan> ActiveBookLoans => _activeBookLoans;

    // for EF
    private LibraryCart() { }

    public LibraryCart(Guid id, Guid membershipId, List<BookLoan.BookLoan> activeBookLoans)
        : base(id)
    {
        _membershipId = membershipId;
        _activeBookLoans = activeBookLoans;
    }
    
    public BookLoan.BookLoan Loan(Guid membershipId, Guid bookId, string bookIsbn)
    {
        BookLoan.BookLoan loan = new (
            membershipId,
            bookId,
            bookIsbn,
            DateTimeOffset.Now.AddDays(14));
        
        if (_activeBookLoans.Count >= 5)
        {
            throw new DomainException("Loan limit exceeded");
        }

        // TODO: load book to check it's isbn
        if (_activeBookLoans.Any(x => x.BookIsbn == loan.BookIsbn))
        {
            throw new DomainException("Cannot loan same book twice");
        }

        _activeBookLoans.Add(loan);
        
        AddDomainEvent(new BookLoanEvent.BookLoaned(
            loan.Id,
            _membershipId,
            loan.ReturnDate));

        return loan;
    }
}