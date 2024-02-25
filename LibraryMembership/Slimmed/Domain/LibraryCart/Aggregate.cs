using System;
using System.Collections.Generic;
using System.Linq;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Shared.Domain.Exceptions;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;

namespace LibraryMembership.Slimmed.Domain.LibraryCart;

public sealed class LibraryCartAggregate : AggregateRoot<Guid>
{
    private readonly Guid _membershipId;
    
    private readonly List<BookLoanEntity> _activeBookLoans;
    public IReadOnlyList<BookLoanEntity> ActiveBookLoans => _activeBookLoans;

    // for EF
    private LibraryCartAggregate() { }

    public LibraryCartAggregate(Guid id, Guid membershipId, List<BookLoanEntity> activeBookLoans)
        : base(id)
    {
        _membershipId = membershipId;
        _activeBookLoans = activeBookLoans;
    }
    
    public void Loan(BookLoanEntity loan)
    {
        if (_activeBookLoans.Count >= 5)
        {
            throw new DomainException("Loan limit exceeded");
        }

        if (_activeBookLoans.Any(x => x.BookIsbn == loan.BookIsbn))
        {
            throw new DomainException("Cannot loan same book twice");
        }

        _activeBookLoans.Add(loan);
        
        AddDomainEvent(new BookLoanEvent.BookLoaned(
            loan.Id,
            _membershipId,
            loan.ReturnDate));
    }
    
    public void Return(Guid bookId)
    {
        BookLoanEntity? bookLoan = _activeBookLoans
            .FirstOrDefault(x => x.Id == bookId);
        
        if (bookLoan is null)
        {
            throw new DomainException("Book loan not found");
        }

        bookLoan.Return();
        
        AddDomainEvent(new BookLoanEvent.BookReturned(
            Id,
            _membershipId));
    }
    
    public void Prolong(Guid bookId)
    {
        BookLoanEntity? bookLoan = _activeBookLoans
            .FirstOrDefault(x => x.Id == bookId);
        
        if (bookLoan is null)
        {
            throw new DomainException("Book loan not found");
        }

        bookLoan.Prolong();
    }
}