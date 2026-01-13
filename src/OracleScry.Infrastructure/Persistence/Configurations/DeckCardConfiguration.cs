using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for DeckCard junction entity.
/// </summary>
public class DeckCardConfiguration : IEntityTypeConfiguration<DeckCard>
{
    public void Configure(EntityTypeBuilder<DeckCard> builder)
    {
        // Composite primary key
        builder.HasKey(dc => new { dc.DeckId, dc.CardId });

        builder.Property(dc => dc.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(dc => dc.IsSideboard)
            .HasDefaultValue(false);

        builder.Property(dc => dc.AddedAt)
            .IsRequired();

        // Relationship to Deck (cascade delete - removing deck removes all cards)
        builder.HasOne(dc => dc.Deck)
            .WithMany(d => d.DeckCards)
            .HasForeignKey(dc => dc.DeckId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship to Card (restrict - can't delete card if in decks)
        builder.HasOne(dc => dc.Card)
            .WithMany()
            .HasForeignKey(dc => dc.CardId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for efficient querying
        builder.HasIndex(dc => dc.DeckId);
        builder.HasIndex(dc => dc.CardId);
        builder.HasIndex(dc => dc.IsSideboard);
    }
}
