using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Abstractions;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

public sealed class LibraryMembershipRepository : ILibraryMembershipRepository
{
    private readonly LibraryMembershipContext _context;
    
    public LibraryMembershipRepository(
        LibraryMembershipContext context)
    {
        _context = context;
    }
    
    public async Task<LibraryMembershipAggregate?> GetAggregateAsync(Guid membershipId, CancellationToken ct)
    {
        return await LoadEntityWithIncludes()
            .Where(x => x.Id == membershipId)
            .Select(x => x.ToAggregate(DateTimeOffset.Now, _context))
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
        
        aggregate.ToEntity(model, _context);
        
        await _context.SaveChangesAsync(ct);
    }

    private IQueryable<LibraryMembershipEntity> LoadEntityWithIncludes()
    {
        return _context.LibraryMemberships
            .Include(x => x.BookLoans)
            .Include(x => x.BookReservations)
            .Include(x => x.Fines);
    }
}