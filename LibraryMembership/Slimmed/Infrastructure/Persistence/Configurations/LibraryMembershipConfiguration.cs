using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class LibraryMembershipConfiguration : IEntityTypeConfiguration<Domain.LibraryMembership.LibraryMembership>
{
    public void Configure(EntityTypeBuilder<Domain.LibraryMembership.LibraryMembership> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        
        builder.Ignore(b => b.DomainEvents);

        builder.HasMany(b => b.Fines)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);
    }
}