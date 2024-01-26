using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryMembership.Slimmed;

public abstract class LibraryMembershipAggregate
{
    private readonly List<BookLoan> _bookLoans;
    private readonly List<BookReservation> _bookReservations;
    private readonly List<Fine> _fines;

    public IReadOnlyList<BookLoan> BookLoans => _bookLoans;
    public IReadOnlyList<BookReservation> BookReservations => _bookReservations;
    public IReadOnlyList<Fine> Fines => _fines;

    private LibraryMembershipAggregate(List<BookLoan> bookLoans, List<BookReservation> bookReservations,
        List<Fine> fines)
    {
        _bookLoans = bookLoans;
        _bookReservations = bookReservations;
        _fines = fines;
    }

    public sealed class Active : LibraryMembershipAggregate
    {
        public Active(List<BookLoan> bookLoans, List<BookReservation> bookReservations, List<Fine> fines)
            : base(bookLoans, bookReservations, fines)
        {
        }

        public void AddLoan(BookLoan loan)
        {
            if (_bookLoans.Count >= 5)
            {
                throw new InvalidOperationException("Loan limit exceeded");
            }

            if (_bookLoans.Any(x => x.BookId == loan.BookId))
            {
                throw new InvalidOperationException("Cannot loan same book twice");
            }

            _bookLoans.Add(loan);
        }

        public void ReturnLoan(Guid loanId)
        {
            BookLoan? loan = _bookLoans.FirstOrDefault(l => l.LoanId == loanId);
            if (loan is null)
            {
                return;
            }

            _bookLoans.Remove(loan);
        }

        // TODO: Should be possible only if there is no reservation for this book
        public void ApplyExtension(Guid loanId, DateTimeOffset now)
        {
            BookLoan? loan = _bookLoans.FirstOrDefault(l => l.LoanId == loanId);
            if (loan is null)
            {
                throw new InvalidOperationException("Loan not found");
            }

            if (loan.IsOverdue(now))
            {
                throw new InvalidOperationException("Cannot extend overdue loan");
            }

            loan.ApplyExtension(now);
        }

        public void AddReservation(BookReservation reservation)
        {
            if (_bookReservations.Any(x => x.BookId == reservation.BookId))
            {
                throw new InvalidOperationException("Cannot reserve same book twice");
            }

            _bookReservations.Add(reservation);
        }

        public void CancelReservation(Guid reservationId)
        {
            BookReservation? reservation = _bookReservations
                .FirstOrDefault(r => r.ReservationId == reservationId);
            if (reservation is null)
            {
                return;
            }

            _bookReservations.Remove(reservation);
        }
    }

    public sealed class Suspended : LibraryMembershipAggregate
    {
        public Suspended(List<BookLoan> bookLoans, List<BookReservation> bookReservations, List<Fine> fines)
            : base(bookLoans, bookReservations, fines)
        {
        }

        public void PayFine(Guid fineId)
        {
            Fine fine = _fines.FirstOrDefault(f => f.FineId == fineId);
            if (fine is null)
            {
                return;
            }
            
            if (fine.IsPaid)
            {
                return;
            }
            
            fine.MarkAsPaid();
        }
    }

    public sealed class Expired : LibraryMembershipAggregate
    {
        public Expired(List<BookLoan> bookLoans, List<BookReservation> bookReservations, List<Fine> fines)
            : base(bookLoans, bookReservations, fines)
        {
        }

        public void PayFine(Guid fineId)
        {
            Fine fine = _fines.FirstOrDefault(f => f.FineId == fineId);
            if (fine is null)
            {
                return;
            }
            
            if (fine.IsPaid)
            {
                return;
            }
            
            fine.MarkAsPaid();
        }

        public void RenewMembership()
        {
        }
    }

    public static LibraryMembershipAggregate Create(List<BookLoan> bookLoans,
        List<BookReservation> bookReservations, List<Fine> fines, DateTimeOffset membershipExpiry, DateTimeOffset now)
    {
        MembershipStatus status = EvaluateMembershipStatus(bookLoans, fines, membershipExpiry, now);

        return status switch
        {
            MembershipStatus.Active => new Active(bookLoans, bookReservations, fines),
            MembershipStatus.Suspended => new Suspended(bookLoans, bookReservations, fines),
            MembershipStatus.Expired => new Expired(bookLoans, bookReservations, fines),
            _ => throw new InvalidOperationException("Invalid membership status")
        };
    }

    private static MembershipStatus EvaluateMembershipStatus(List<BookLoan> bookLoans, List<Fine> fines,
        DateTimeOffset membershipExpiry, DateTimeOffset now)
    {
        if (membershipExpiry < now)
        {
            return MembershipStatus.Expired;
        }

        return bookLoans.Any(l => l.IsOverdue(now)) || fines.Any(f => !f.IsPaid)
            ? MembershipStatus.Suspended
            : MembershipStatus.Active;
    }
}