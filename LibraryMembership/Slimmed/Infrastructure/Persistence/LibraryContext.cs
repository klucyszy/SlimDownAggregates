using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
        SeedData();
    }

    public DbSet<Entities.LibraryMembership> LibraryMemberships { get; set; }
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

        LibraryCarts.Add(new LibraryCart
        {
            Id = Guid.Parse("66fdfb6a-bcb9-49e2-86be-19816695051e"),
            MembershipId = Guid.Parse("66fdfb6a-bcb9-49e2-86be-19816695051e")
        });
        
        SaveChanges();
    }
}