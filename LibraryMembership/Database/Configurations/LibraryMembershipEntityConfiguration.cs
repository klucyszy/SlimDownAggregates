using LibraryMembership.Slimmed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Database.Configurations;

public sealed class LibraryMembershipEntityConfiguration : IEntityTypeConfiguration<LibraryMembershipModel>
{
    public void Configure(EntityTypeBuilder<LibraryMembershipModel> builder)
    {
        builder.HasKey(b => b.MembershipId);

        builder.HasMany<FineModel>(b => b.Fines)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);

        builder.HasMany<BookLoanModel>(b => b.BookLoans)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);
        
        builder.HasMany<BookReservationModel>(b => b.BookReservations)
            .WithOne()
            .HasForeignKey(b => b.MembershipId);
    }
}

public sealed class BookLoanEntityConfiguration : IEntityTypeConfiguration<BookLoanModel>
{
    public void Configure(EntityTypeBuilder<BookLoanModel> builder)
    {
        builder.HasKey(b => b.LoanId);
        builder.HasIndex(b => new { b. BookId, b.MembershipId }).IsUnique();
        builder.Ignore(b => b.EntityState);
    }
}

public sealed class FineEntityConfiguration : IEntityTypeConfiguration<FineModel>
{
    public void Configure(EntityTypeBuilder<FineModel> builder)
    {
        builder.HasKey(b => b.FineId);
    }
}

public sealed class BookReservationEntityConfiguration : IEntityTypeConfiguration<BookReservationModel>
{
    public void Configure(EntityTypeBuilder<BookReservationModel> builder)
    {
        builder.HasKey(b => b.BookId);
    }
}