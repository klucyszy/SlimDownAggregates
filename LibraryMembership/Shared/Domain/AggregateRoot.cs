using System;
using System.Collections.Generic;

namespace LibraryMembership.Shared.Domain;

public abstract class AggregateRoot
{
    public Guid Id { get; }
    
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    protected AggregateRoot(Guid id)
    {
        Id = id;
    }
    
    protected AggregateRoot() { }

    protected void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearEvents()
        => _domainEvents.Clear();
    
}