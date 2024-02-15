using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Infrastructure.Persistence;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed;

public sealed class LibraryMembershipRepository : ILibraryMembershipRepository
{
    private readonly DataContext _dataContext;

    public LibraryMembershipRepository(
        DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<LibraryMembershipAggregate?> GetAggregateAsync(Guid membershipId)
    {
        return await GetModel()
            .Where(x => x.Id == membershipId)
            .Select(x => x.ToAggregate(DateTimeOffset.Now, _dataContext))
            .FirstOrDefaultAsync();
    }
    
    public async Task UpdateAsync(LibraryMembershipAggregate aggregate)
    {
        LibraryMembershipEntity? model = await _dataContext.LibraryMemberships
            .FindAsync(aggregate.Id);
        
        
        model?.ToModel(aggregate, _dataContext);
        
        _dataContext.SaveChangesAsync();
    }

    private IQueryable<LibraryMembershipEntity> GetModel()
    {
        return _dataContext.LibraryMemberships
            .Include(x => x.BookLoans)
            .Include(x => x.BookReservations)
            .Include(x => x.Fines);
    }
}