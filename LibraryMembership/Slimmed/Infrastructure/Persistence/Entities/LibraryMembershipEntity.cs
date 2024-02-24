using System;
using System.Collections.Generic;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public class LibraryMembershipEntity
{
    public enum MembershipStatus
    {
        Active,
        Suspended,
        Expired
    }
    
    public Guid Id { get; set; }
    public MembershipStatus Status { get; set; }
    public List<FineEntity> Fines { get; set; } = [];
    public DateTimeOffset MembershipExpiry { get; set; }
}