using System;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public class BookReservationEntity
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public DateTime ReservationDate { get; private set; }
    public Guid MembershipId { get; private set; }

    public BookReservationEntity(Guid id, Guid bookId, DateTime reservationDate)
    {
        Id = id;
        BookId = bookId;
        ReservationDate = reservationDate;
    }
}