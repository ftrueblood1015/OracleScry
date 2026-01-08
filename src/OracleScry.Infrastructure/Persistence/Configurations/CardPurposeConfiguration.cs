using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for CardPurpose entity.
/// </summary>
public class CardPurposeConfiguration : IEntityTypeConfiguration<CardPurpose>
{
    public void Configure(EntityTypeBuilder<CardPurpose> builder)
    {
        builder.HasKey(cp => cp.Id);

        // String properties
        builder.Property(cp => cp.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(cp => cp.Slug)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(cp => cp.Description)
            .HasMaxLength(500);

        builder.Property(cp => cp.Patterns)
            .HasMaxLength(2000);

        // Category as string for readability
        builder.Property(cp => cp.Category)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        // Indexes
        builder.HasIndex(cp => cp.Slug).IsUnique();
        builder.HasIndex(cp => cp.Category);
        builder.HasIndex(cp => cp.IsActive);
        builder.HasIndex(cp => cp.DisplayOrder);
    }
}
