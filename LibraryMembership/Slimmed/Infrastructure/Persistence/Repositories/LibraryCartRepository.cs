using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.LibraryCart;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

internal sealed class LibraryCartRepository : IAggregateRepository<LibraryCartAggregate>
{
    private readonly LibraryContext _context;

    public LibraryCartRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<LibraryCartAggregate> GetAggregateAsync(Guid id, CancellationToken ct)
    {
        return await Queryable()
            .Where(a => a.Id == id)
            .Select(e => e.ToAggregate())
            .FirstOrDefaultAsync(ct);
    }

    public async Task UpdateAsync(LibraryCartAggregate aggregate, CancellationToken ct = default)
    {
        LibraryCart entity = await _context.LibraryCarts.FindAsync(aggregate.Id, ct);

        entity.MapFrom(aggregate, _context);
        
        await _context.SaveChangesAsync(ct);
    }

    private IQueryable<LibraryCart> Queryable()
    {
        return _context.LibraryCarts
            .Include(e => e.BookLoans.Where(bl => bl.Returned == false));
    }
}