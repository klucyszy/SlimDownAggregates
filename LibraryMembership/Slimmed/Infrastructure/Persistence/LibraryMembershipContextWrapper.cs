using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence;

public class LibraryMembershipContextWrapper : ILibraryMembershipContext
{
    private readonly LibraryMembershipContext _context;
    private readonly IEnumerable<ISaveChangesInterceptor> _onSaveChangesInterceptors;

    public LibraryMembershipContext Context => _context;
    
    public LibraryMembershipContextWrapper(LibraryMembershipContext context,
        IEnumerable<ISaveChangesInterceptor> onSaveChangesInterceptor)
    {
        _context = context;
        _onSaveChangesInterceptors = onSaveChangesInterceptor;
    }
    
    public DbSet<LibraryMembershipEntity> LibraryMemberships => _context.LibraryMemberships;
    public DbSet<BookLoanEntity> BookLoans => _context.BookLoans;
    public DbSet<FineEntity> Fines => _context.Fines;
    public DbSet<BookReservationEntity> BookReservations => _context.BookReservations;
    
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (ISaveChangesInterceptor interceptor in _onSaveChangesInterceptors)
        {
            await interceptor.OnSaveChangesAsync(_context, ct);
        }
        
        return await _context.SaveChangesAsync(ct);
    }
}