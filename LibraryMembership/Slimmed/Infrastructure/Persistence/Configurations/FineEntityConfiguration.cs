using LibraryMembership.Slimmed.Domain.LibraryMembership.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryMembership.Slimmed.Infrastructure.Persistence.Configurations;

public sealed class FineEntityConfiguration : IEntityTypeConfiguration<FineEntity>
{
    public void Configure(EntityTypeBuilder<FineEntity> builder)
    {
        builder.HasKey(b => b.Id);
    }
}