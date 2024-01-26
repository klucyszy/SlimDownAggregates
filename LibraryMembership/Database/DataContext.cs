using LibraryMembership.Slimmed;
using Microsoft.EntityFrameworkCore;

namespace LibraryMembership.Database;

public class DataContext : DbContext
{
    public DbSet<LibraryMembershipModel> LibraryMemberships { get; set; }
}