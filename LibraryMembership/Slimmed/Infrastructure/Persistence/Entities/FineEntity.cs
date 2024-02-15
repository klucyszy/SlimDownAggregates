using System;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public class FineEntity
{
    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsPaid { get; private set; }
    public Guid MembershipId { get; private set; }

    public FineEntity(Guid id, decimal amount)
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