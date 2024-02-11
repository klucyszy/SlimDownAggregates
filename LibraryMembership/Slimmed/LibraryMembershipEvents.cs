using System;

namespace LibraryMembership.Slimmed;

public abstract record LibraryMembershipEvent
{
    public record LoanAdded(
        Guid MembershipId,
        Guid LoanId,
        Guid BookId,
        DateTimeOffset LoanDate)
    : LibraryMembershipEvent;
}