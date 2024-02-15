using System;

namespace LibraryMembership.Slimmed.Domain.LibraryMembership;

public abstract record LibraryMembershipEvent
{
    public record BookLoaned(
        Guid MembershipId,
        Guid LoanId,
        Guid BookId,
        DateTimeOffset LoanDate)
    : LibraryMembershipEvent;
}