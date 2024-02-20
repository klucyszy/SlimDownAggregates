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
        
        builder.HasMany<BookReservationEntity>(b => b.BookReservations)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);
    }
}

public sealed class BookLoanEntityConfiguration : IEntityTypeConfiguration<BookLoanEntity>
{
    public void Configure(EntityTypeBuilder<BookLoanEntity> builder)
    {
        builder.HasKey(b => b.Id);
        builder.HasIndex(b => new { b. BookId, b.MembershipId }).IsUnique();
    }
}

public sealed class FineEntityConfiguration : IEntityTypeConfiguration<FineEntity>
{
    public void Configure(EntityTypeBuilder<FineEntity> builder)
    {
        builder.HasKey(b => b.Id);
    }
}

public sealed class BookReservationEntityConfiguration : IEntityTypeConfiguration<BookReservationEntity>
{
    public void Configure(EntityTypeBuilder<BookReservationEntity> builder)
    {
        builder.HasKey(b => b.BookId);
    }
}