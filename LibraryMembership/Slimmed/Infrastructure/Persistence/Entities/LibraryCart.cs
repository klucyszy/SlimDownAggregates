using System;
using System.Collections.Generic;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public sealed class LibraryCart
{
    public Guid Id { get; set; }
    public Guid MembershipId { get; set; }
    public List<BookLoan> BookLoans { get; set; } = [];
}