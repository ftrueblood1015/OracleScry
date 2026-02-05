using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for DeckTemplateCard junction entity.
/// </summary>
public class DeckTemplateCardConfiguration : IEntityTypeConfiguration<DeckTemplateCard>
{
    public void Configure(EntityTypeBuilder<DeckTemplateCard> builder)
    {
        // Composite primary key
        builder.HasKey(dtc => new { dtc.DeckTemplateId, dtc.CardId });

        builder.Property(dtc => dtc.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(dtc => dtc.IsSideboard)
            .HasDefaultValue(false);

        builder.Property(dtc => dtc.IsCommander)
            .HasDefaultValue(false);

        // Relationship to DeckTemplate (cascade delete - removing template removes all cards)
        builder.HasOne(dtc => dtc.DeckTemplate)
            .WithMany(dt => dt.TemplateCards)
            .HasForeignKey(dtc => dtc.DeckTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship to Card (restrict - can't delete card if in templates)
        builder.HasOne(dtc => dtc.Card)
            .WithMany()
            .HasForeignKey(dtc => dtc.CardId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for efficient querying
        builder.HasIndex(dtc => dtc.DeckTemplateId);
        builder.HasIndex(dtc => dtc.CardId);
        builder.HasIndex(dtc => dtc.IsSideboard);
        builder.HasIndex(dtc => dtc.IsCommander);
    }
}
