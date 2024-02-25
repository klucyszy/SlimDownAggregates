using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class BookLoanEntityConfiguration : IEntityTypeConfiguration<BookLoanEntity>
{
    public void Configure(EntityTypeBuilder<BookLoanEntity> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => new { BookId = b. BookIsbn, b.MembershipId }).IsUnique();
    }
}