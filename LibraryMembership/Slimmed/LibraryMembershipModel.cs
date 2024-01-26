using System;
using System.Collections.Generic;

namespace LibraryMembership.Slimmed;

public class LibraryMembershipModel
{
    public Guid MembershipId { get; set; }
    public MembershipStatus Status { get; set; }
    public List<BookLoan> BookLoans { get; set; }
    public List<BookReservation> BookReservations { get; set; }
    public List<Fine> Fines { get; set; }
    public DateTime MembershipExpiry { get; set; }
}

public class BookLoan
{
    public Guid LoanId { get; private set; }
    public Guid BookId { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public bool ExtensionApplied { get; private set; }

    public BookLoan(Guid loanId, Guid bookId, DateTimeOffset dueDate)
    {
        LoanId = loanId;
        BookId = bookId;
        DueDate = dueDate;
        ExtensionApplied = false;
    }

    public bool IsOverdue(DateTimeOffset now)
    {
        return DueDate < now;
    }

    public void ApplyExtension(DateTimeOffset now)
    {
        ExtensionApplied = true;
        DueDate = now.AddDays(14);
    }
}

public class BookReservation
{
    public Guid ReservationId { get; private set; }
    public Guid BookId { get; private set; }
    public DateTime ReservationDate { get; private set; }

    public BookReservation(Guid reservationId, Guid bookId, DateTime reservationDate)
    {
        ReservationId = reservationId;
        BookId = bookId;
        ReservationDate = reservationDate;
    }
}

public class Fine
{
    public Guid FineId { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsPaid { get; private set; }

    public Fine(Guid fineId, decimal amount)
    {
        FineId = fineId;
        Amount = amount;
        IsPaid = false;
    }

    // Methods to handle fine payment
    public void MarkAsPaid()
    {
        IsPaid = true;
    }
}