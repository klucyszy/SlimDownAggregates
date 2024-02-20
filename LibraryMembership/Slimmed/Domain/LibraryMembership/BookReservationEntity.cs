using System;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership;

public class BookReservationEntity
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public DateTimeOffset ReservationDate { get; private set; }
    public Guid MembershipId { get; private set; }

    public BookReservationEntity(Guid id, Guid bookId, Guid membershipId, DateTimeOffset reservationDate)
    {
        Id = id;
        BookId = bookId;
        MembershipId = membershipId;
        ReservationDate = reservationDate;
    }
}