using LibraryMembership.Slimmed.Domain.BookLoan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class BookLoanConfiguration : IEntityTypeConfiguration<BookLoan>
{
    public void Configure(EntityTypeBuilder<BookLoan> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        
        builder.Ignore(b => b.DomainEvents);
        
        builder.HasIndex(b => new { b.BookIsbn, MembershipId = b.LoanedById }).IsUnique();
    }
}