using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibraryMembership.Slimmed.Domain.LibraryCart;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence;

public class LibraryMembershipContext : DbContext
{
    public LibraryMembershipContext(DbContextOptions<LibraryMembershipContext> options) : base(options)
    {
        SeedData();
    }

    public DbSet<LibraryMembershipAggregate> LibraryMemberships { get; set; }
    public DbSet<LibraryCartAggregate> LibraryCarts { get; set; }
    public DbSet<FineEntity> Fines { get; set; }

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
        
        LibraryCarts.Add(new LibraryCartAggregate(
            Guid.Parse("66fdfb6a-bcb9-49e2-86be-19816695051e"),
            Guid.Parse("66fdfb6a-bcb9-49e2-86be-19816695051e"),
            new List<BookLoanEntity>()));

        SaveChanges();
    }
}