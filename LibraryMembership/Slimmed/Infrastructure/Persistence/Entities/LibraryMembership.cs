using System;
using System.Collections.Generic;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;

public sealed class LibraryMembership
{
    public Guid Id { get; set; }
    public List<Fine> Fines { get; set; }
}