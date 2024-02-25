using System;
using System.Collections.Generic;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Slimmed.Domain.LibraryCart;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership;

public class LibraryMembershipAggregate : AggregateRoot<Guid>
{
    private readonly List<FineEntity> _fines;
    public IReadOnlyList<FineEntity> Fines => _fines;
    
    public LibraryMembershipAggregate() {}
    
    private LibraryMembershipAggregate(
        Guid membershipId,
        List<FineEntity> fines)
        : base(membershipId)
    {
        _fines = fines;
    }

    public sealed class Active(
        Guid membershipId,
        List<FineEntity> fines)
        : LibraryMembershipAggregate(membershipId, fines)
    {
    }

    public sealed class Suspended(
        Guid membershipId,
        List<FineEntity> fines)
        : LibraryMembershipAggregate(membershipId, fines)
    {
    }

    public sealed class Expired(
        Guid membershipId,
        List<FineEntity> fines)
        : LibraryMembershipAggregate(membershipId, fines)
    {
    }
}

public class BookLoanModel
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public bool ExtensionApplied { get; private set; }

    public BookLoanModel(Guid id, Guid bookId, DateTimeOffset dueDate, bool extensionApplied)
    {
        Id = id;
        BookId = bookId;
        DueDate = dueDate;
        ExtensionApplied = extensionApplied;
    }
    
    public BookLoanModel(Guid id, Guid bookId, DateTimeOffset dueDate) : this(
        id, bookId, dueDate, false)
    {
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