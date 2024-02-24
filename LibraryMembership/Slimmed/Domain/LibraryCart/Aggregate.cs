using System;
using System.Collections.Generic;
using System.Linq;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Shared.Domain.Exceptions;

namespace LibraryMembership.Slimmed.Domain.LibraryCart;

public sealed class LibraryCartAggregate : AggregateRoot<Guid>
{
    private readonly Guid _membershipId;
    private readonly List<BookLoanForAggregate> _activeBookLoans;
    public LibraryCartAggregate(Guid id, Guid membershipId, List<BookLoanForAggregate> activeBookLoans)
        : base(id)
    {
        _membershipId = membershipId;
        _activeBookLoans = activeBookLoans;
    }
    
    public void Loan(BookLoanForAggregate loan)
    {
        if (_activeBookLoans.Count >= 5)
        {
            throw new DomainException("Loan limit exceeded");
        }

        if (_activeBookLoans.Any(x => x.BookId == loan.BookId))
        {
            throw new DomainException("Cannot loan same book twice");
        }

        _activeBookLoans.Add(loan);
        
        AddDomainEvent(new BookLoanEvent.BookLoaned(
            loan.BookId,
            _membershipId,
            loan.ReturnDate));
    }
    
    public void Return(Guid bookId)
    {
        BookLoanForAggregate? bookLoan = _activeBookLoans
            .FirstOrDefault(x => x.BookId == bookId);
        
        if (bookLoan is null)
        {
            throw new DomainException("Book loan not found");
        }

        bookLoan.Returned = true;
        
        AddDomainEvent(new BookLoanEvent.BookReturned(
            Id,
            _membershipId));
    }
    
    public void Prolong(Guid bookId, DateTimeOffset newReturnDate)
    {
        BookLoanForAggregate? bookLoan = _activeBookLoans
            .FirstOrDefault(x => x.BookId == bookId);
        
        if (bookLoan is null)
        {
            throw new DomainException("Book loan not found");
        }

        bookLoan.ReturnDate = newReturnDate;
    }
}

public sealed class BookLoanForAggregate
{
    public Guid BookId { get; set; }
    public DateTimeOffset ReturnDate { get; set; }
    public bool Returned { get; set; }
}