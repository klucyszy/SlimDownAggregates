using LibraryMembership.Slimmed.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class LibraryCartConfiguration : IEntityTypeConfiguration<LibraryCart>
{
    public void Configure(EntityTypeBuilder<LibraryCart> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        
        builder.HasMany(b => b.BookLoans)
            .WithOne()
            .HasForeignKey(b => b.LoanedById);
    }
}