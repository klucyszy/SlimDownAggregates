using System;
using System.Collections.Generic;
using System.Linq;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed;

public static class LibraryMembershipFactory
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

    public static LibraryMembershipEntity ToModel(this LibraryMembershipEntity entity,
        LibraryMembershipAggregate aggregate, DataContext _context)
    {
        entity.Status = aggregate switch
        {
            LibraryMembershipAggregate.Active => MembershipStatus.Active,
            LibraryMembershipAggregate.Suspended => MembershipStatus.Suspended,
            LibraryMembershipAggregate.Expired => MembershipStatus.Expired,
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


public static class ListUpdater
{
    public static void Update<TLTo, TLFrom>(
        this List<TLTo> to,
        List<TLFrom> from,
        Func<TLFrom, Func<TLTo, bool>> matches,
        Func<TLFrom, TLTo> onAdd,
        DbContext context
    )
    {
        var toUpdate = from.Where(f => to.Any(matches(f))).ToList();
        var toAdd = from.Except(toUpdate);
    
        foreach (var updated in toUpdate)
        {
            var current = to.Single(matches(updated));
            //onUpdate(current, updated);
        }

        foreach (var added in toAdd)
        {
            var bla = onAdd(added);
            to.Add(bla);
            context.Add(bla);
        }
    }
}
