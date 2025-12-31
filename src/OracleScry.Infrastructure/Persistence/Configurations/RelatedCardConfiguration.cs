using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for RelatedCard entity.
/// </summary>
public class RelatedCardConfiguration : IEntityTypeConfiguration<RelatedCard>
{
    public void Configure(EntityTypeBuilder<RelatedCard> builder)
    {
        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.Name).HasMaxLength(300).IsRequired();
        builder.Property(rc => rc.TypeLine).HasMaxLength(200).IsRequired();
        builder.Property(rc => rc.Component).HasMaxLength(50).IsRequired();
        builder.Property(rc => rc.Uri).HasMaxLength(500).IsRequired();
        builder.Property(rc => rc.Object).HasMaxLength(20).IsRequired();

        // Index on CardId for efficient joins
        builder.HasIndex(rc => rc.CardId);
        builder.HasIndex(rc => rc.RelatedCardScryfallId);
    }
}
