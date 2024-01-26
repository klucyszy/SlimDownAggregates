using LibraryMembership.Slimmed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryMembership.Database.Repositories;

public sealed class LibraryMembershipRepository
{
    private readonly DataContext _dataContext;

    public LibraryMembershipRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<LibraryMembershipModel?> GetAsync(Guid membershipId)
    {
        return await _dataContext.LibraryMemberships
            .Include(x => x.BookLoans)
            .Include(x => x.BookReservations)
            .Include(x => x.Fines)
            .Where(x => x.MembershipId == membershipId)
            .FirstOrDefaultAsync();
    }

    public void UpdateAsync(LibraryMembershipModel membershipAggregate)
    {
        _dataContext.LibraryMemberships.Update(membershipAggregate);
    }
}