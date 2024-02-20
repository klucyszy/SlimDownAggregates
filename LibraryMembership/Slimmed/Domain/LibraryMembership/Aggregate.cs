using System;
using System.Collections.Generic;
using System.Linq;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership;

public abstract class LibraryMembershipAggregate : AggregateRoot<Guid>
{
    private readonly List<BookLoan> _bookLoans;
    public IReadOnlyList<BookLoan> BookLoans => _bookLoans;
    
    private readonly List<BookReservationEntity> _bookReservations;
    public IReadOnlyList<BookReservationEntity> BookReservations => _bookReservations;
    
    private readonly List<FineEntity> _fines;
    public IReadOnlyList<FineEntity> Fines => _fines;
    
    private LibraryMembershipAggregate(
        Guid membershipId,
        List<BookLoan> bookLoans,
        List<BookReservationEntity> bookReservations,
        List<FineEntity> fines)
        : base(membershipId)
    {
        _bookLoans = bookLoans;
        _bookReservations = bookReservations;
        _fines = fines;
    }

    public sealed class Active(
        Guid membershipId,
        List<BookLoan> bookLoans,
        List<BookReservationEntity> bookReservations,
        List<FineEntity> fines)
        : LibraryMembershipAggregate(membershipId, bookLoans, bookReservations, fines)
    {
        public void LoanBook(BookLoan loanEntity)
        {
            if (_bookLoans.Count >= 5)
            {
                throw new InvalidOperationException("Loan limit exceeded");
            }

            if (_bookLoans.Any(x => x.BookId == loanEntity.BookId))
            {
                throw new InvalidOperationException("Cannot loan same book twice");
            }

            _bookLoans.Add(loanEntity);
            AddDomainEvent(new LibraryMembershipEvent.BookLoaned(
                Id,
                loanEntity.Id,
                loanEntity.BookId,
                DateTimeOffset.Now)
            );
        }
        
        // TODO: Should be possible only if there is no reservationModel for this book
        public void ProlongLoan(Guid loanId, DateTimeOffset now)
        {
            BookLoan? loan = _bookLoans.FirstOrDefault(l => l.Id == loanId);
            if (loan is null)
            {
                throw new InvalidOperationException("Loan not found");
            }

            if (loan.IsOverdue(now))
            {
                throw new InvalidOperationException("Cannot extend overdue loanModel");
            }

            loan.ApplyExtension(now);
        }

        public void ReserveBook(BookReservationEntity reservationEntity)
        {
            if (_bookReservations.Any(x => x.BookId == reservationEntity.BookId))
            {
                throw new InvalidOperationException("Cannot reserve same book twice");
            }

            _bookReservations.Add(reservationEntity);
        }

        public void CancelReservation(Guid bookId)
        {
            BookReservationEntity? reservation = _bookReservations
                .FirstOrDefault(r => r.BookId == bookId);
            if (reservation is null)
            {
                return;
            }

            _bookReservations.Remove(reservation);
        }
    }

    public sealed class Suspended(
        Guid membershipId,
        List<BookLoan> bookLoans,
        List<BookReservationEntity> bookReservations,
        List<FineEntity> fines)
        : LibraryMembershipAggregate(membershipId, bookLoans, bookReservations, fines)
    {
        public void PayFine(Guid fineId)
        {
            FineEntity fineEntity = _fines.FirstOrDefault(f => f.Id == fineId);
            if (fineEntity is null)
            {
                return;
            }
            
            if (fineEntity.IsPaid)
            {
                return;
            }
            
            fineEntity.MarkAsPaid();
        }
    }

    public sealed class Expired(
        Guid membershipId,
        List<BookLoan> bookLoans,
        List<BookReservationEntity> bookReservations,
        List<FineEntity> fines)
        : LibraryMembershipAggregate(membershipId, bookLoans, bookReservations, fines)
    {
        public void PayFine(Guid fineId)
        {
            FineEntity fineEntity = _fines.FirstOrDefault(f => f.Id == fineId);
            if (fineEntity is null)
            {
                return;
            }
            
            if (fineEntity.IsPaid)
            {
                return;
            }
            
            fineEntity.MarkAsPaid();
        }

        public void RenewMembership()
        {
        }
    }
    
    public void ReturnBook(Guid bookId)
    {
        BookLoan? loan = _bookLoans.FirstOrDefault(l => l.BookId == bookId);
        if (loan is null)
        {
            return;
        }

        _bookLoans.Remove(loan);
    }

    public static LibraryMembershipAggregate Create(Guid membershipId, List<BookLoan> bookLoans,
        List<BookReservationEntity> bookReservations, List<FineEntity> fines, DateTimeOffset membershipExpiry,
        DateTimeOffset now)
    {
        LibraryMembershipEntity.MembershipStatus status = EvaluateMembershipStatus(bookLoans, fines, membershipExpiry, now);
        return status switch
        {
            LibraryMembershipEntity.MembershipStatus.Active => new Active(membershipId, bookLoans, bookReservations, fines),
            LibraryMembershipEntity.MembershipStatus.Suspended => new Suspended(membershipId, bookLoans, bookReservations, fines),
            LibraryMembershipEntity.MembershipStatus.Expired => new Expired(membershipId, bookLoans, bookReservations, fines),
            _ => throw new InvalidOperationException("Invalid membership status")
        };
    }

    private static LibraryMembershipEntity.MembershipStatus EvaluateMembershipStatus(List<BookLoan> bookLoans, List<FineEntity> fines,
        DateTimeOffset membershipExpiry, DateTimeOffset now)
    {
        if (membershipExpiry < now)
        {
            return LibraryMembershipEntity.MembershipStatus.Expired;
        }

        return bookLoans.Any(l => l.IsOverdue(now)) || fines.Any(f => !f.IsPaid)
            ? LibraryMembershipEntity.MembershipStatus.Suspended
            : LibraryMembershipEntity.MembershipStatus.Active;
    }
}

public class BookLoan
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public bool ExtensionApplied { get; private set; }

    public BookLoan(Guid id, Guid bookId, DateTimeOffset dueDate, bool extensionApplied)
    {
        Id = id;
        BookId = bookId;
        DueDate = dueDate;
        ExtensionApplied = extensionApplied;
    }
    
    public BookLoan(Guid id, Guid bookId, DateTimeOffset dueDate) : this(
        id, bookId, dueDate, false)
    {
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