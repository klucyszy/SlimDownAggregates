using System;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Slimmed.Domain.BookLoan;

namespace LibraryMembership.Slimmed.Domain.LibraryCart;

public abstract record LibraryCartsEvents : IDomainEvent
{
    public sealed record BookLoaned(
        Guid BookId,
        Guid LoanedById,
        DateTimeOffset ReturnDate)
        : BookLoanEvent;
}