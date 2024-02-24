using System;
using System.Linq;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;

public static class LibraryMembershipMapper
{
    public static LibraryMembershipAggregate ToAggregate(this LibraryMembershipEntity entity,
        DateTimeOffset now)
    {
        return LibraryMembershipAggregate.Create(
            entity.Id,
            entity.Fines.ToList(), 
            entity.MembershipExpiry,
            now);
    }

    public static LibraryMembershipEntity ToEntity(this LibraryMembershipAggregate aggregate,
        LibraryMembershipEntity entity)
    {
        entity.Status = aggregate switch
        {
            LibraryMembershipAggregate.Active => LibraryMembershipEntity.MembershipStatus.Active,
            LibraryMembershipAggregate.Suspended => LibraryMembershipEntity.MembershipStatus.Suspended,
            LibraryMembershipAggregate.Expired => LibraryMembershipEntity.MembershipStatus.Expired,
            _ => throw new InvalidOperationException("Invalid membership status")
        };
        entity.Fines = aggregate.Fines.ToList();

        return entity;
    }
}