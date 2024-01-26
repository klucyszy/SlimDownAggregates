using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryMembership.Original;

public class LibraryMembership
{
    public Guid MembershipId { get; private set; }
    public MembershipStatus Status { get; private set; }
    private List<BookLoan> _bookLoans;
    private List<BookReservation> _bookReservations;
    private List<Fine> _fines;
    private DateTime _membershipExpiry;

    public LibraryMembership(Guid membershipId, DateTime membershipExpiry)
    {
        MembershipId = membershipId;
        _membershipExpiry = membershipExpiry;
        _bookLoans = new List<BookLoan>();
        _bookReservations = new List<BookReservation>();
        _fines = new List<Fine>();
        Status = MembershipStatus.Active;
    }

    public void AddLoan(BookLoan loan)
    {
        EnsureActiveMembership();
        EnsureNoUnresolvedFines();
        EnsureLoanLimitNotExceeded();

        _bookLoans.Add(loan);
        UpdateMembershipStatus();
    }

    public void ReturnLoan(Guid loanId)
    {
        BookLoan? loan = _bookLoans.FirstOrDefault(l => l.LoanId == loanId);
        if (loan == null)
            throw new InvalidOperationException("Loan not found");

        _bookLoans.Remove(loan);
        UpdateMembershipStatus();
    }

    public void AddReservation(BookReservation reservation)
    {
        EnsureActiveMembership();
        _bookReservations.Add(reservation);
    }

    public void PayFine(Guid fineId)
    {
        Fine? fine = _fines.FirstOrDefault(f => f.FineId == fineId);
        if (fine == null)
            throw new InvalidOperationException("Fine not found");

        fine.MarkAsPaid();
        UpdateMembershipStatus();
    }

    // Invariant: Member cannot have more than 5 active loans
    private void EnsureLoanLimitNotExceeded()
    {
        if (_bookLoans.Count >= 5)
            throw new InvalidOperationException("Loan limit exceeded");
    }

    // Invariant: Member must have no unresolved fines
    private void EnsureNoUnresolvedFines()
    {
        if (_fines.Any(f => !f.IsPaid))
            throw new InvalidOperationException("Unpaid fines exist");
    }

    // Invariant: Membership must be active
    private void EnsureActiveMembership()
    {
        if (Status != MembershipStatus.Active)
            throw new InvalidOperationException("Membership is not active");
    }

    // Update membership status based on current state
    private void UpdateMembershipStatus()
    {
        if (_bookLoans.Any(l => l.IsOverdue) || _fines.Any(f => !f.IsPaid))
        {
            Status = MembershipStatus.Suspended;
        }
        else
        {
            Status = MembershipStatus.Active;
        }
    }
}

public class BookLoan
{
    public Guid LoanId { get; private set; }
    public Guid BookId { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool ExtensionApplied { get; private set; }

    public BookLoan(Guid loanId, Guid bookId, DateTime dueDate)
    {
        LoanId = loanId;
        BookId = bookId;
        DueDate = dueDate;
        ExtensionApplied = false;
    }

    // Methods related to book loan (e.g., ExtendDueDate)
    public bool IsOverdue => DateTime.Now > DueDate;
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

    // Methods related to book reservation
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

public enum MembershipStatus
{
    Active,
    Suspended,
    Expired
}
