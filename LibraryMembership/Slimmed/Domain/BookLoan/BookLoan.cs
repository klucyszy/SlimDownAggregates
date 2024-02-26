using System;
using LibraryMembership.Shared.Domain;

namespace LibraryMembership.Slimmed.Domain.BookLoan;

public class BookLoan : AggregateRoot
{
    public Guid MembershipId { get; private set; }
    public Guid BookId { get; private set; }
    public string BookIsbn { get; private set; }
    public int ProlongedTimes { get; private set; }
    public bool Returned { get; private set; }
    public DateTimeOffset ReturnDate { get; private set; }
    
    public BookLoan() { }
    
    public BookLoan(Guid membershipId, Guid bookId, string bookIsbn, DateTimeOffset returnDate)
    {
        MembershipId = membershipId;
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
            MembershipId,
            ReturnDate));
    }
    
    public void Prolong(Guid bookId)
    {
        ProlongedTimes++;
        ReturnDate = DateTimeOffset.Now;
        
        AddDomainEvent(new BookLoanEvent.BookProlonged(
            bookId,
            MembershipId,
            ReturnDate));
    }
    

    public bool IsOverdue(DateTimeOffset now)
    {
        return ReturnDate < now;
    }

    public void Prolong()
    {
        ProlongedTimes++;
        ReturnDate = DateTimeOffset.Now;
    }
}