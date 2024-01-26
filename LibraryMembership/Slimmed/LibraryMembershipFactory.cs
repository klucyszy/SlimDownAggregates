using System;
using System.Linq;

namespace LibraryMembership.Slimmed;

public static class LibraryMembershipFactory
{
    public static LibraryMembershipAggregate ToAggregate(this LibraryMembershipModel model,
        DateTimeOffset now)
    {
        return LibraryMembershipAggregate.Create(
            model.BookLoans,
            model.BookReservations,
            model.Fines, 
            model.MembershipExpiry,
            now);
    }

    public static LibraryMembershipModel ToModel(this LibraryMembershipAggregate aggregate,
        LibraryMembershipModel model)
    {
        model.Status = aggregate switch
        {
            LibraryMembershipAggregate.Active => MembershipStatus.Active,
            LibraryMembershipAggregate.Suspended => MembershipStatus.Suspended,
            LibraryMembershipAggregate.Expired => MembershipStatus.Expired,
            _ => throw new InvalidOperationException("Invalid membership status")
        };
        model.BookLoans = aggregate.BookLoans.ToList();
        model.BookReservations = aggregate.BookReservations.ToList();
        model.Fines = aggregate.Fines.ToList();

        return model;
    }
}