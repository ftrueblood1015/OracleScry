using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for CardCardPurpose junction table.
/// </summary>
public class CardCardPurposeConfiguration : IEntityTypeConfiguration<CardCardPurpose>
{
    public void Configure(EntityTypeBuilder<CardCardPurpose> builder)
    {
        // Composite primary key
        builder.HasKey(ccp => new { ccp.CardId, ccp.CardPurposeId });

        // Confidence precision
        builder.Property(ccp => ccp.Confidence)
            .HasPrecision(3, 2); // 0.00 - 1.00

        builder.Property(ccp => ccp.MatchedPattern)
            .HasMaxLength(500);

        builder.Property(ccp => ccp.AssignedBy)
            .HasMaxLength(100)
            .IsRequired();

        // Relationships
        builder.HasOne(ccp => ccp.Card)
            .WithMany(c => c.CardPurposes)
            .HasForeignKey(ccp => ccp.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ccp => ccp.CardPurpose)
            .WithMany(cp => cp.CardPurposes)
            .HasForeignKey(ccp => ccp.CardPurposeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for efficient querying
        builder.HasIndex(ccp => ccp.CardId);
        builder.HasIndex(ccp => ccp.CardPurposeId);
        builder.HasIndex(ccp => ccp.AssignedAt);
    }
}
