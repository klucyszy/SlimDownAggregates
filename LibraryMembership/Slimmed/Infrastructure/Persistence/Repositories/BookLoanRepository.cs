using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Shared.Domain.Abstractions;
using LibraryMembership.Slimmed.Domain.BookLoan;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Repositories;

internal sealed class BookLoanRepository : IAggregateRepository<BookLoan>
{
    private readonly LibraryContext _context;

    public BookLoanRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<BookLoan> GetAggregateAsync(Guid id, CancellationToken ct)
    {
        return await Queryable()
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    public async Task UpdateAsync(BookLoan aggregate, bool saveChanges = true, CancellationToken ct = default)
    {
        if (saveChanges)
        {
            await _context.SaveChangesAsync(ct);
        }
    }

    private IQueryable<BookLoan> Queryable()
    {
        return _context.BookLoans;
    }
}