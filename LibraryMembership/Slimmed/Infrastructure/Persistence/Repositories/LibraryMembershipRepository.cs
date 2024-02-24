using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Infrastructure.Abstractions;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

public sealed class LibraryMembershipRepository : IAggregateRepository<LibraryMembershipAggregate>
{
    private readonly LibraryMembershipContext _context;
    
    public LibraryMembershipRepository(
        LibraryMembershipContext context)
    {
        _context = context;
    }
    
    public async Task<LibraryMembershipAggregate?> GetAggregateAsync(Guid id, CancellationToken ct)
    {
        return await LoadEntityWithIncludes()
            .Where(x => x.Id == id)
            .Select(x => x.ToAggregate(DateTimeOffset.Now))
            .FirstOrDefaultAsync(ct);
    }
    
    public async Task UpdateAsync(LibraryMembershipAggregate aggregate, CancellationToken ct)
    {
        LibraryMembershipEntity? model = await _context.LibraryMemberships
            .FindAsync(aggregate.Id);

        if (model is null)
        {
            throw new InvalidOperationException("Membership not found");
        }
        
        aggregate.ToEntity(model);
        
        await _context.SaveChangesAsync(ct);
    }

    private IQueryable<LibraryMembershipEntity> LoadEntityWithIncludes()
    {
        return _context.LibraryMemberships
            .Include(x => x.Fines);
    }
}