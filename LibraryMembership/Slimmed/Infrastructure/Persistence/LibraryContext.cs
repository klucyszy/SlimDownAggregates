using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibraryMembership.Slimmed.Domain.BookLoan;
using LibraryMembership.Slimmed.Domain.LibraryCart;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
        SeedData();
    }

    public DbSet<Domain.LibraryMembership.LibraryMembership> LibraryMemberships { get; set; }
    public DbSet<LibraryCart> LibraryCarts { get; set; }
    public DbSet<BookLoan> BookLoans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void SeedData()
    {
        if (LibraryCarts.Any())
        {
            return;
        }
        
        LibraryCarts.Add(new LibraryCart(
            Guid.Parse("66fdfb6a-bcb9-49e2-86be-19816695051e"),
            Guid.Parse("66fdfb6a-bcb9-49e2-86be-19816695051e"),
            new List<BookLoan>()));

        SaveChanges();
    }
}