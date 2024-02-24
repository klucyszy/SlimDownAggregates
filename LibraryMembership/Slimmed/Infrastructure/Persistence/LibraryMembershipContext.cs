using System;
using System.Linq;
using System.Reflection;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence;

public class LibraryMembershipContext : DbContext
{
    public LibraryMembershipContext(DbContextOptions<LibraryMembershipContext> options) : base(options)
    {
        SeedData();
    }

    public DbSet<LibraryMembershipEntity> LibraryMemberships { get; set; }
    public DbSet<FineEntity> Fines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void SeedData()
    {
        if (LibraryMemberships.Any())
        {
            return;
        }
        
        LibraryMemberships.Add(new LibraryMembershipEntity
        {
            Id = Guid.Parse("66fdfb6a-bcb9-49e2-86be-19816695051e"),
            Status = LibraryMembershipEntity.MembershipStatus.Active,
            MembershipExpiry = DateTimeOffset.Now.AddYears(1)
        });

        SaveChanges();
    }
}