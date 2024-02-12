using LibraryMembership.Slimmed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryMembership.Database.Repositories;

public sealed class LibraryMembershipRepository
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
            .Where(x => x.MembershipId == membershipId)
            .Select(x => x.ToAggregate(DateTimeOffset.Now))
            .FirstOrDefaultAsync();
    }
    
    public async Task UpdateAsync(LibraryMembershipAggregate aggregate)
    {
        LibraryMembershipModel? model = await _dataContext.LibraryMemberships
            .FindAsync(aggregate.Id);
        
        if (model is not null)
        {
            aggregate.ToModel(model);
            
            _dataContext.LibraryMemberships.Update(model);

            foreach (BookLoanModel loans in model.BookLoans)
            {
                if (loans.EntityState == EntityState.Added)
                {
                    _dataContext.BookLoans.Add(loans);
                }
                else if (loans.EntityState == EntityState.Deleted)
                {
                    _dataContext.BookLoans.Remove(loans);
                }
            }
        }
    }

    private IQueryable<LibraryMembershipModel> GetModel()
    {
        return _dataContext.LibraryMemberships
            .Include(x => x.BookLoans)
            .Include(x => x.BookReservations)
            .Include(x => x.Fines);
    }
}