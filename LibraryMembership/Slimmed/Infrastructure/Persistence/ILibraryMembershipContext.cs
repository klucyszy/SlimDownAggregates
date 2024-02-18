using System.Threading;
using System.Threading.Tasks;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence;

public interface ILibraryMembershipContext
{
    public LibraryMembershipContext Context { get; }
    
    public DbSet<LibraryMembershipEntity> LibraryMemberships { get; }
    public DbSet<BookLoanEntity> BookLoans { get; }
    public DbSet<FineEntity> Fines { get; }
    public DbSet<BookReservationEntity> BookReservations { get; }
    
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}