using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.BookLoan;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

internal sealed class BookLoanRepository : IAggregateRepository<BookLoanAggregate>
{
    private readonly LibraryContext _context;

    public BookLoanRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<BookLoanAggregate> GetAggregateAsync(Guid id, CancellationToken ct)
    {
        return await Queryable()
            .Where(e => e.Id == id)
            .Select(e => e.ToAggregate())
            .FirstOrDefaultAsync(ct);
    }

    public async Task UpdateAsync(BookLoanAggregate aggregate, CancellationToken ct = default)
    {
        BookLoan entity = await _context.BookLoans.FindAsync(new { aggregate.Id }, ct);

        entity.MapFrom(aggregate);
        
        await _context.SaveChangesAsync(ct);
    }

    private IQueryable<BookLoan> Queryable()
    {
        return _context.BookLoans;
    }
}