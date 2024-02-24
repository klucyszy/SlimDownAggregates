using System.Collections.Generic;

namespace LibraryMembership.Shared.Domain;

public abstract class AggregateRoot<TId>
{
    public TId Id { get; }
    
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearEvents()
        => _domainEvents.Clear();
    
}