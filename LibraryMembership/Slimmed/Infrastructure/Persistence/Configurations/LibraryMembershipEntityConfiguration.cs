using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class LibraryMembershipEntityConfiguration : IEntityTypeConfiguration<LibraryMembershipEntity>
{
    public void Configure(EntityTypeBuilder<LibraryMembershipEntity> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasMany<FineEntity>(b => b.Fines)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);

        builder.HasMany<BookLoanEntity>(b => b.BookLoans)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);
    }
}