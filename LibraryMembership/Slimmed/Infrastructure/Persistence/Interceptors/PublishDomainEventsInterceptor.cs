using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Domain;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Wolverine;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Interceptors;

public interface ISaveChangesInterceptor
{
    Task OnSaveChangesAsync<TContext>(TContext context, CancellationToken ct = default) where TContext : DbContext;
}

public sealed class PublishDomainEventsInterceptor : ISaveChangesInterceptor
{
    private readonly IMessageBus _bus;

    public PublishDomainEventsInterceptor(IMessageBus bus)
    {
        _bus = bus;
    }

    public async Task OnSaveChangesAsync<TContext>(TContext context, CancellationToken ct = default) where TContext : DbContext
    {
        // handle for all types of ids ? or do I even need this?
        List<IDomainEvent>? domainEvents = context.ChangeTracker?.Entries()
            .OfType<IAggregateRoot>()
            .SelectMany(x => x.DomainEvents)
            .ToList();

        if (domainEvents is null || domainEvents.Count == 0)
        {
            return;
        }
        
        foreach(var domainEvent in domainEvents)
        {
            await _bus.SendAsync(domainEvent);
        }
    }
}