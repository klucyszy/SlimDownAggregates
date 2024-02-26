using System;
using System.Collections.Generic;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership;

public class LibraryMembership : AggregateRoot
{
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