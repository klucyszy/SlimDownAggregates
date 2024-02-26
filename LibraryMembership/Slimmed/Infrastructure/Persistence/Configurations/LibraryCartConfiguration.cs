using LibraryMembership.Slimmed.Domain.BookLoan;
using LibraryMembership.Slimmed.Domain.LibraryCart;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class LibraryCartConfiguration : IEntityTypeConfiguration<LibraryCart>
{
    public void Configure(EntityTypeBuilder<LibraryCart> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasMany<BookLoan>(b => b.ActiveBookLoans)
            .WithOne()
            .HasForeignKey(b => b.Id);
    }
}