using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class LibraryMembershipConfiguration : IEntityTypeConfiguration<Entities.LibraryMembership>
{
    public void Configure(EntityTypeBuilder<Entities.LibraryMembership> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        
        builder.HasMany(b => b.Fines)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);
    }
}