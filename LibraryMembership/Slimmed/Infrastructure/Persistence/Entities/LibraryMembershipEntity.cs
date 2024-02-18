using System;
using System.Collections.Generic;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public interface IAggregateRoot
{
    public List<IDomainEvent> DomainEvents { get; }
}

public class LibraryMembershipEntity : IAggregateRoot
{
    public enum MembershipStatus
    {
        Active,
        Suspended,
        Expired
    }
    
    public Guid Id { get; set; }
    public MembershipStatus Status { get; set; }
    public List<BookLoanEntity> BookLoans { get; set; } = [];
    public List<BookReservationEntity> BookReservations { get; set; } = [];
    public List<FineEntity> Fines { get; set; } = [];
    public DateTimeOffset MembershipExpiry { get; set; }
    
    public EntityState EntityState { get; set; }

    public List<IDomainEvent> DomainEvents { get; } = [];
}