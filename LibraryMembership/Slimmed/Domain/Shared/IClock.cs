using System;

namespace LibraryMembership.Slimmed.Domain.Shared;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
    DateTimeOffset Now { get; }
}

internal sealed class Clock : IClock
{
    public DateTimeOffset UtcNow { get; } = DateTimeOffset.UtcNow;
    public DateTimeOffset Now { get; } = DateTimeOffset.Now;
}