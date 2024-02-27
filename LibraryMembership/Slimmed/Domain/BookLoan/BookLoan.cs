using System;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Shared.Domain.Exceptions;

namespace LibraryMembership.Slimmed.Domain.BookLoan;

public class BookLoan : AggregateRoot
{
    public Guid LoanedById { get; private set; }
    public Guid BookId { get; private set; }
    public string BookIsbn { get; private set; }
    public int ProlongedTimes { get; private set; }
    public bool Returned { get; private set; }
    public DateTimeOffset ReturnDate { get; private set; }
    
    private BookLoan() { }
    
    public BookLoan(Guid loanedById, Guid bookId, string bookIsbn, DateTimeOffset returnDate)
    {
        LoanedById = loanedById;
        BookId = bookId;
        BookIsbn = bookIsbn;
        ReturnDate = returnDate;
    }
    
    public void Return()
    {
        Returned = true;
        ReturnDate = DateTimeOffset.Now;
        
        AddDomainEvent(new BookLoanEvent.BookReturned(
            Id,
            LoanedById,
            ReturnDate));
    }
    
    public void Prolong(Guid bookId)
    {
        ProlongedTimes++;
        ReturnDate = DateTimeOffset.Now;
        
        AddDomainEvent(new BookLoanEvent.BookProlonged(
            bookId,
            LoanedById,
            ReturnDate));
    }
    

    public bool IsOverdue(DateTimeOffset now)
    {
        return ReturnDate < now;
    }

    public void Prolong()
    {
        if (ProlongedTimes > 0)
        {
            throw new DomainException("Book was already prolonged");
        }
        
        ProlongedTimes++;
        ReturnDate = DateTimeOffset.Now;
    }
}