using System;
using System.Linq;
using LibraryMembership.Shared.Infrastructure.Persistence;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;

public static class LibraryMembershipMapper
{
    public static LibraryMembershipAggregate ToAggregate(this LibraryMembershipEntity entity,
        DateTimeOffset now, DataContext _context)
    {
        return LibraryMembershipAggregate.Create(
            _context,
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
        LibraryMembershipEntity entity, DataContext _context)
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
            a => e => e.Id == a.Id,
            a => new BookLoanEntity(a.Id, a.BookId, entity.Id, a.DueDate),
            _context
        );
        
        entity.BookReservations = aggregate.BookReservations.ToList();
        entity.Fines = aggregate.Fines.ToList();

        return entity;
    }
}