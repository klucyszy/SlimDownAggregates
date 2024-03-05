using System;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public sealed class Fine
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public Guid MembershipId { get; set; }
}