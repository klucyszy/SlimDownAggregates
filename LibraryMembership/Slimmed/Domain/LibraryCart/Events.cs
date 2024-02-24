using System;
using LibraryMembership.Shared.Domain;

namespace LibraryMembership.Slimmed.Domain.LibraryCart;

public abstract record BookLoanEvent : IDomainEvent
{
    public sealed record BookLoaned(
        Guid BookId,
        Guid LoanedById,
        DateTimeOffset ReturnDate)
        : BookLoanEvent;
    
    public sealed record BookReturned(
        Guid BookId,
        Guid ReturnedById)
        : BookLoanEvent;
}