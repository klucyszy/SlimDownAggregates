using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.LibraryCart;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

internal sealed class LibraryCartRepository : IAggregateRepository<LibraryCart>
{
    private readonly LibraryContext _context;

    public LibraryCartRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<LibraryCart> GetAggregateAsync(Guid id, CancellationToken ct)
    {
        return await Queryable()
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    public async Task UpdateAsync(LibraryCart aggregate, bool saveChanges = true, CancellationToken ct = default)
    {
        if (saveChanges)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
    
    private IQueryable<LibraryCart> Queryable()
    {
        return _context.LibraryCarts
            .Include(b => b.ActiveBookLoans.Where(bl => bl.Returned == false));
    }
}