using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LibraryMembership.Slimmed;

public class LibraryMembershipModel
{
    public Guid MembershipId { get; set; }
    public MembershipStatus Status { get; set; }
    public List<BookLoanModel> BookLoans { get; set; }
    public List<BookReservationModel> BookReservations { get; set; }
    public List<FineModel> Fines { get; set; }
    public DateTimeOffset MembershipExpiry { get; set; }
    
    public EntityState EntityState { get; set; }
}

public class BookLoanModel
{
    public Guid LoanId { get; private set; }
    public Guid BookId { get; private set; }
    public Guid MembershipId { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public bool ExtensionApplied { get; private set; }

    public BookLoanModel(Guid loanId, Guid bookId, DateTimeOffset dueDate)
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

public class BookReservationModel
{
    public Guid ReservationId { get; private set; }
    public Guid BookId { get; private set; }
    public DateTime ReservationDate { get; private set; }
    public Guid MembershipId { get; private set; }

    public BookReservationModel(Guid reservationId, Guid bookId, DateTime reservationDate)
    {
        ReservationId = reservationId;
        BookId = bookId;
        ReservationDate = reservationDate;
    }
}

public class FineModel
{
    public Guid FineId { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsPaid { get; private set; }
    public Guid MembershipId { get; private set; }

    public FineModel(Guid fineId, decimal amount)
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