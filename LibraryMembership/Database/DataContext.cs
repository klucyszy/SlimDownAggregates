using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibraryMembership.Slimmed;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        SeedData();
    }

    public DbSet<LibraryMembershipModel> LibraryMemberships { get; set; }
    public DbSet<BookLoanModel> BookLoans { get; set; }
    public DbSet<FineModel> Fines { get; set; }
    public DbSet<BookReservationModel> BookReservations { get; set; }

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
        
        LibraryMemberships.Add(new LibraryMembershipModel
        {
            MembershipId = Guid.Parse("66fdfb6a-bcb9-49e2-86be-19816695051e"),
            Status = MembershipStatus.Active,
            MembershipExpiry = DateTimeOffset.Now.AddYears(1)
        });

        SaveChanges();
    }
}