using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for DeckTemplate entity.
/// </summary>
public class DeckTemplateConfiguration : IEntityTypeConfiguration<DeckTemplate>
{
    public void Configure(EntityTypeBuilder<DeckTemplate> builder)
    {
        builder.HasKey(dt => dt.Id);

        builder.Property(dt => dt.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(dt => dt.Description)
            .HasMaxLength(2000);

        builder.Property(dt => dt.Format)
            .HasMaxLength(50);

        builder.Property(dt => dt.SetCode)
            .HasMaxLength(10);

        builder.Property(dt => dt.SetName)
            .HasMaxLength(100);

        builder.Property(dt => dt.ScryfallDeckId)
            .HasMaxLength(100);

        builder.Property(dt => dt.IsActive)
            .HasDefaultValue(true);

        // Indexes for efficient querying
        builder.HasIndex(dt => dt.Name);
        builder.HasIndex(dt => dt.Format);
        builder.HasIndex(dt => dt.SetCode);
        builder.HasIndex(dt => dt.IsActive);
    }
}
