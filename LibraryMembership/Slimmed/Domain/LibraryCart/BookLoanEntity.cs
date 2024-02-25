using System;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;

public class BookLoanEntity
{
    public Guid Id { get; private set; }
    public string BookIsbn { get; private set; }
    public Guid MembershipId { get; private set; }
    public int ProlongedTimes { get; private set; }
    public bool Returned { get; private set; }
    public DateTimeOffset ReturnDate { get; private set; }

    public BookLoanEntity(Guid id, string bookIsbn, Guid membershipId,
        DateTimeOffset returnDate)
    {
        Id = id;
        BookIsbn = bookIsbn;
        MembershipId = membershipId;
        ReturnDate = returnDate;
    }

    public void Return()
    {
        Returned = true;
        ReturnDate = DateTimeOffset.Now;
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