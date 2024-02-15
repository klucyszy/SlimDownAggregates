using System;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public class BookLoanEntity
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public Guid MembershipId { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public bool ExtensionApplied { get; private set; }

    public BookLoanEntity(Guid id, Guid bookId, Guid membershipId, DateTimeOffset dueDate)
    {
        Id = id;
        BookId = bookId;
        MembershipId = membershipId;
        DueDate = dueDate;
        ExtensionApplied = false;
    }

    public bool IsOverdue(DateTimeOffset now)
    {
        return DueDate < now;
    }

    public void ApplyExtension(DateTimeOffset now)
    {
        ExtensionApplied = true;
        DueDate = now.AddDays(14);
    }
}