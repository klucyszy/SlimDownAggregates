using System;
using LibraryMembership.Shared.Domain;

namespace LibraryMembership.Slimmed.Domain.BookLoan;

public abstract record BookLoanEvent : IDomainEvent
{
    public sealed record BookLoaned(
        Guid BookId,
        Guid LoanedById,
        DateTimeOffset ReturnDate)
        : BookLoanEvent;
    
    public sealed record BookReturned(
        Guid BookId,
        Guid ReturnedById,
        DateTimeOffset ReturnDate)
        : BookLoanEvent;

    public sealed record BookProlonged(
        Guid BookId,
        Guid ReturnedById,
        DateTimeOffset ReturnDate)
        : BookLoanEvent;
}