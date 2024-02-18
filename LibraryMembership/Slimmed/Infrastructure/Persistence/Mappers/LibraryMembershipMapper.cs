using System;
using System.Linq;
using LibraryMembership.Shared.Infrastructure.Persistence;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;

public static class LibraryMembershipMapper
{
    public static LibraryMembershipAggregate ToAggregate(this LibraryMembershipEntity entity,
        DateTimeOffset now)
    {
        return LibraryMembershipAggregate.Create(
            entity.Id,
            entity.BookLoans
                .Select(b => new BookLoan(b.Id, b.BookId, b.DueDate, b.ExtensionApplied))
                .ToList(),
            entity.BookReservations.ToList(),
            entity.Fines.ToList(), 
            entity.MembershipExpiry,
            now);
    }

    public static LibraryMembershipEntity ToEntity(this LibraryMembershipAggregate aggregate,
        LibraryMembershipEntity entity, ILibraryMembershipContext _context)
    {
        entity.Status = aggregate switch
        {
            LibraryMembershipAggregate.Active => LibraryMembershipEntity.MembershipStatus.Active,
            LibraryMembershipAggregate.Suspended => LibraryMembershipEntity.MembershipStatus.Suspended,
            LibraryMembershipAggregate.Expired => LibraryMembershipEntity.MembershipStatus.Expired,
            _ => throw new InvalidOperationException("Invalid membership status")
        };
        entity.BookLoans.Update(
            aggregate.BookLoans.ToList(),
            (entity, aggregate) => entity.BookId == aggregate.BookId,
            aggregate => new BookLoanEntity(aggregate.Id, aggregate.BookId, entity.Id, aggregate.DueDate),
            _context.Context);
        entity.BookReservations.Update(
            aggregate.BookReservations.ToList(),
            (entity, aggregate) => entity.BookId == aggregate.BookId,
            aggregate => new BookReservationEntity(aggregate.Id, aggregate.BookId, entity.Id, aggregate.ReservationDate),
            _context.Context);
        entity.Fines = aggregate.Fines.ToList();
        entity.DomainEvents.AddRange(aggregate.DomainEvents);

        return entity;
    }
}