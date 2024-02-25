using LibraryMembership.Slimmed.Domain.LibraryCart;
using LibraryMembership.Slimmed.Domain.LibraryMembership;
using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class LibraryCartAggregateConfiguration : IEntityTypeConfiguration<LibraryCartAggregate>
{
    public void Configure(EntityTypeBuilder<LibraryCartAggregate> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasMany<BookLoanEntity>(b => b.ActiveBookLoans)
            .WithOne()
            .HasForeignKey(b => b.Id);
    }
}