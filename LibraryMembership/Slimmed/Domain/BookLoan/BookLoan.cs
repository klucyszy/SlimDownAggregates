using System;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Shared.Domain.Exceptions;
using LibraryMembership.Slimmed.Domain.Shared;

namespace LibraryMembership.Slimmed.Domain.BookLoan;

public class BookLoan : AggregateRoot
{
    public Guid LoanedById { get; private set; }
    public bool Returned { get; private set; }
    public DateTimeOffset? ReturnDate { get; private set; }
    
    private BookLoan() { }
    
    public BookLoan(Guid loanedById, DateTimeOffset? returnDate)
    {
        LoanedById = loanedById;
        ReturnDate = returnDate;
    }
    
    public void Return(IClock clock)
    {
        
        
        Returned = true;
        ReturnDate = clock.Now;
        
        AddDomainEvent(new BookLoanEvent.BookReturned(
            Id,
            LoanedById,
            ReturnDate.Value));
    }
}