using System;
using System.Collections.Generic;
using LibraryMembership.Shared.Domain;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership;

public sealed class LibraryMembership : AggregateRoot
{
    public enum MembershipStatus
    {
        Active,
        Suspended
    }
    
    private readonly List<Fine> _fines;
    public IReadOnlyList<Fine> Fines => _fines;
    
    public LibraryMembership() {}
    
    private LibraryMembership(
        Guid membershipId,
        List<Fine> fines)
        : base(membershipId)
    {
        _fines = fines;
    }
}