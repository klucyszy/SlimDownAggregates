using System;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;

public class Fine

{
    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsPaid { get; private set; }
    public Guid MembershipId { get; private set; }

    public Fine(Guid id, decimal amount)
    {
        Id = id;
        Amount = amount;
        IsPaid = false;
    }

    // Methods to handle fine payment
    public void MarkAsPaid()
    {
        IsPaid = true;
    }
}