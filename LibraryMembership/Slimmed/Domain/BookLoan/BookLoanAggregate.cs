using System;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Slimmed.Domain.Shared;

namespace LibraryMembership.Slimmed.Domain.BookLoan;

public class BookLoanAggregate : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public Guid LoanedById { get; private set; }
    public bool Returned { get; private set; }
    public DateTimeOffset? ReturnDate { get; private set; }
    
    public BookLoanAggregate(Guid id, Guid bookId, Guid loanedById, DateTimeOffset? returnDate)
    {
        Id = id;
        BookId = bookId;
        LoanedById = loanedById;
        ReturnDate = returnDate;
    }
    
    public void Return(IClock clock)
    {
        Returned = true;
        ReturnDate = clock.Now;
        
        AddDomainEvent(new BookLoanEvent.BookReturned(
            BookId,
            LoanedById,
            ReturnDate.Value));
    }
}