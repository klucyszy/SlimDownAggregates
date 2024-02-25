using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class LibraryMembershipEntityConfiguration : IEntityTypeConfiguration<LibraryMembershipAggregate>
{
    public void Configure(EntityTypeBuilder<LibraryMembershipAggregate> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasMany<FineEntity>(b => b.Fines)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);
    }
}