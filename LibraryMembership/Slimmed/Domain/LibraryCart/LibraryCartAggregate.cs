using System;
using System.Collections.Generic;
using System.Linq;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Shared.Domain.Exceptions;
using LibraryMembership.Slimmed.Domain.BookLoan;
using LibraryMembership.Slimmed.Domain.Shared;

namespace LibraryMembership.Slimmed.Domain.LibraryCart;

public sealed class LibraryCartAggregate : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid MembershipId { get; private set; }
    
    private readonly List<ActiveBookLoan> _activeBookLoans;
    public IReadOnlyList<ActiveBookLoan> ActiveBookLoans => _activeBookLoans;
    
    public LibraryCartAggregate(Guid id, Guid membershipId, List<ActiveBookLoan> activeBookLoans)
        : base(id)
    {
        Id = id;
        MembershipId = membershipId;
        _activeBookLoans = activeBookLoans;
    }
    
    public BookLoanEvent.BookLoaned Loan(Guid bookId, string bookIsbn, IClock clock)
    {
        ActiveBookLoan loan = new(
            bookId,
            bookIsbn,
            clock.Now.AddDays(14),
            0);
        
        if (_activeBookLoans.Count >= 5)
        {
            throw new DomainException("Loan limit exceeded");
        }
        
        if (_activeBookLoans.Any(x => x.BookIsbn == loan.BookIsbn))
        {
            throw new DomainException("Cannot loan same book twice");
        }

        _activeBookLoans.Add(loan);
        
        var result = new BookLoanEvent.BookLoaned(
            
            loan.BookId,
            MembershipId,
            loan.ReturnDate);
        
        AddDomainEvent(result);
        
        return result;
    }

    public void Prolong(Guid bookId)
    {
        ActiveBookLoan loan = _activeBookLoans.FirstOrDefault(x => x.BookId == bookId);
        
        if (loan is null)
        {
            throw new DomainException("Book not found");
        }
        
        if (loan.Prolongations >= 1)
        {
            throw new DomainException("Prolongation limit exceeded");
        }
        
        loan.Prolong();
        
        AddDomainEvent(new BookLoanEvent.BookProlonged(
            loan.BookId,
            MembershipId,
            loan.ReturnDate));
    }
    
    public sealed class ActiveBookLoan
    {
        public Guid BookId { get; init; }
        public string BookIsbn { get; init; }
        public DateTimeOffset ReturnDate { get; private set; }
        public int Prolongations { get; private set; } = 0;

        public ActiveBookLoan(Guid bookId, string bookIsbn, DateTimeOffset returnDate, int prolongations)
        {
            BookId = bookId;
            BookIsbn = bookIsbn;
            ReturnDate = returnDate;
            Prolongations = prolongations;
        }
        
        public void Prolong()
        {
            if (Prolongations >= 1)
            {
                throw new DomainException("Prolongation limit exceeded");
            }
            
            Prolongations++;
            ReturnDate = ReturnDate.AddDays(14);
        }
    }
}