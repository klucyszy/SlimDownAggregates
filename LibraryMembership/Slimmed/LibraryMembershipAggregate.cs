using System;
using System.Collections.Generic;
using System.Linq;
using LibraryMembership.Database;

namespace LibraryMembership.Slimmed;

public abstract class LibraryMembershipAggregate
{
    private readonly DataContext _context;
    private readonly List<BookLoanModel> _bookLoans;
    private readonly List<BookReservationModel> _bookReservations;
    private readonly List<FineModel> _fines;
    private readonly Guid _membershipId;

    public Guid Id => _membershipId;
    public IReadOnlyList<BookLoanModel> BookLoans => _bookLoans;
    public IReadOnlyList<BookReservationModel> BookReservations => _bookReservations;
    public IReadOnlyList<FineModel> Fines => _fines;

    private LibraryMembershipAggregate(DataContext context, Guid membershipId, List<BookLoanModel> bookLoans,
        List<BookReservationModel> bookReservations, List<FineModel> fines)
    {
        _context = context;
        _membershipId = membershipId;
        _bookLoans = bookLoans;
        _bookReservations = bookReservations;
        _fines = fines;
    }

    public sealed class Active : LibraryMembershipAggregate
    {
        public Active(DataContext context, Guid membershipId, List<BookLoanModel> bookLoans, List<BookReservationModel> bookReservations, List<FineModel> fines)
            : base(context, membershipId, bookLoans, bookReservations, fines)
        {
        }

        public LibraryMembershipEvent.BookLoaned LoanBook(BookLoanModel loanModel)
        {
            if (_bookLoans.Count >= 5)
            {
                throw new InvalidOperationException("Loan limit exceeded");
            }

            if (_bookLoans.Any(x => x.BookId == loanModel.BookId))
            {
                throw new InvalidOperationException("Cannot loan same book twice");
            }

            _bookLoans.Add(loanModel);
            _context.BookLoans.Add(loanModel);

            return new LibraryMembershipEvent.BookLoaned(
                _membershipId,
                loanModel.LoanId,
                loanModel.BookId,
                DateTimeOffset.Now);
        }

        public void ReturnBook(Guid loanId)
        {
            BookLoanModel? loan = _bookLoans.FirstOrDefault(l => l.LoanId == loanId);
            if (loan is null)
            {
                return;
            }

            _bookLoans.Remove(loan);
        }

        // TODO: Should be possible only if there is no reservationModel for this book
        public void ExtendBookLoan(Guid loanId, DateTimeOffset now)
        {
            BookLoanModel? loan = _bookLoans.FirstOrDefault(l => l.LoanId == loanId);
            if (loan is null)
            {
                throw new InvalidOperationException("Loan not found");
            }

            if (loan.IsOverdue(now))
            {
                throw new InvalidOperationException("Cannot extend overdue loanModel");
            }

            loan.ApplyExtension(now);
            _context.BookLoans.Update(loan);
        }

        public void AddReservation(BookReservationModel reservationModel)
        {
            if (_bookReservations.Any(x => x.BookId == reservationModel.BookId))
            {
                throw new InvalidOperationException("Cannot reserve same book twice");
            }

            _bookReservations.Add(reservationModel);
        }

        public void CancelReservation(Guid reservationId)
        {
            BookReservationModel? reservation = _bookReservations
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
        public Suspended(DataContext context, Guid membershipId, List<BookLoanModel> bookLoans, List<BookReservationModel> bookReservations, List<FineModel> fines)
            : base(context, membershipId, bookLoans, bookReservations, fines)
        {
        }

        public void PayFine(Guid fineId)
        {
            FineModel fineModel = _fines.FirstOrDefault(f => f.FineId == fineId);
            if (fineModel is null)
            {
                return;
            }
            
            if (fineModel.IsPaid)
            {
                return;
            }
            
            fineModel.MarkAsPaid();
        }
    }

    public sealed class Expired : LibraryMembershipAggregate
    {
        public Expired(DataContext context, Guid membershipId, List<BookLoanModel> bookLoans, List<BookReservationModel> bookReservations, List<FineModel> fines)
            : base(context, membershipId, bookLoans, bookReservations, fines)
        {
        }

        public void PayFine(Guid fineId)
        {
            FineModel fineModel = _fines.FirstOrDefault(f => f.FineId == fineId);
            if (fineModel is null)
            {
                return;
            }
            
            if (fineModel.IsPaid)
            {
                return;
            }
            
            fineModel.MarkAsPaid();
        }

        public void RenewMembership()
        {
        }
    }

    public static LibraryMembershipAggregate Create(DataContext context, Guid membershipId, List<BookLoanModel> bookLoans,
        List<BookReservationModel> bookReservations, List<FineModel> fines, DateTimeOffset membershipExpiry,
        DateTimeOffset now)
    {
        MembershipStatus status = EvaluateMembershipStatus(bookLoans, fines, membershipExpiry, now);
        return status switch
        {
            MembershipStatus.Active => new Active(context, membershipId, bookLoans, bookReservations, fines),
            MembershipStatus.Suspended => new Suspended(context, membershipId, bookLoans, bookReservations, fines),
            MembershipStatus.Expired => new Expired(context, membershipId, bookLoans, bookReservations, fines),
            _ => throw new InvalidOperationException("Invalid membership status")
        };
    }

    private static MembershipStatus EvaluateMembershipStatus(List<BookLoanModel> bookLoans, List<FineModel> fines,
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