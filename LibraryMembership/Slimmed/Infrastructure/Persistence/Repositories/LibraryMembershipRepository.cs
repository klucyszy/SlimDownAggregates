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
    private readonly DataContext _dataContext;

    public LibraryMembershipRepository(
        DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<LibraryMembershipAggregate?> GetAggregateAsync(Guid membershipId, CancellationToken ct)
    {
        return await GetModel()
            .Where(x => x.Id == membershipId)
            .Select(x => x.ToAggregate(DateTimeOffset.Now, _dataContext))
            .FirstOrDefaultAsync();
    }
    
    public async Task UpdateAsync(LibraryMembershipAggregate aggregate, CancellationToken ct)
    {
        LibraryMembershipEntity? model = await _dataContext.LibraryMemberships
            .FindAsync(aggregate.Id);
        
        aggregate?.ToEntity(model, _dataContext);
        
        await _dataContext.SaveChangesAsync(ct);
    }

    private IQueryable<LibraryMembershipEntity> GetModel()
    {
        return _dataContext.LibraryMemberships
            .Include(x => x.BookLoans)
            .Include(x => x.BookReservations)
            .Include(x => x.Fines);
    }
}