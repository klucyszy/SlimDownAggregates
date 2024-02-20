using System;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership;

public abstract record LibraryMembershipEvent : IDomainEvent
{
    public record BookLoaned(
        Guid MembershipId,
        Guid LoanId,
        Guid BookId,
        DateTimeOffset LoanDate)
    : LibraryMembershipEvent;
    
    public record BookReturned(
        Guid MembershipId,
        Guid LoanId,
        Guid BookId,
        DateTimeOffset LoanDate)
        : LibraryMembershipEvent;
}