using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public class LibraryMembershipEntity
{
    public Guid Id { get; set; }
    public MembershipStatus Status { get; set; }
    public List<BookLoanEntity> BookLoans { get; set; } = [];
    public IList<BookReservationEntity> BookReservations { get; set; } = new List<BookReservationEntity>();
    public IList<FineEntity> Fines { get; set; } = new List<FineEntity>();
    public DateTimeOffset MembershipExpiry { get; set; }
    
    public EntityState EntityState { get; set; }
}