using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Card entity.
/// Configures JSON columns, indexes, and relationships.
/// </summary>
public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(c => c.Id);

        // Required string properties
        builder.Property(c => c.Name).HasMaxLength(300).IsRequired();
        builder.Property(c => c.Lang).HasMaxLength(10).IsRequired();
        builder.Property(c => c.Layout).HasMaxLength(50).IsRequired();
        builder.Property(c => c.TypeLine).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Rarity).HasMaxLength(20).IsRequired();
        builder.Property(c => c.SetCode).HasMaxLength(10).IsRequired();
        builder.Property(c => c.SetName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.CollectorNumber).HasMaxLength(20).IsRequired();
        builder.Property(c => c.BorderColor).HasMaxLength(20).IsRequired();
        builder.Property(c => c.Frame).HasMaxLength(20).IsRequired();
        builder.Property(c => c.ImageStatus).HasMaxLength(20).IsRequired();
        builder.Property(c => c.SetType).HasMaxLength(50).IsRequired();
        builder.Property(c => c.Object).HasMaxLength(20).IsRequired();

        // Optional string properties
        builder.Property(c => c.ManaCost).HasMaxLength(100);
        builder.Property(c => c.OracleText).HasMaxLength(2000);
        builder.Property(c => c.Power).HasMaxLength(10);
        builder.Property(c => c.Toughness).HasMaxLength(10);
        builder.Property(c => c.Loyalty).HasMaxLength(10);
        builder.Property(c => c.Defense).HasMaxLength(10);
        builder.Property(c => c.Artist).HasMaxLength(200);
        builder.Property(c => c.FlavorName).HasMaxLength(200);
        builder.Property(c => c.FlavorText).HasMaxLength(1000);
        builder.Property(c => c.PrintedName).HasMaxLength(300);
        builder.Property(c => c.PrintedText).HasMaxLength(2000);
        builder.Property(c => c.PrintedTypeLine).HasMaxLength(200);
        builder.Property(c => c.SecurityStamp).HasMaxLength(20);
        builder.Property(c => c.Watermark).HasMaxLength(50);
        builder.Property(c => c.HandModifier).HasMaxLength(10);
        builder.Property(c => c.LifeModifier).HasMaxLength(10);

        // URI properties
        builder.Property(c => c.Uri).HasMaxLength(500);
        builder.Property(c => c.ScryfallUri).HasMaxLength(500);
        builder.Property(c => c.PrintsSearchUri).HasMaxLength(500);
        builder.Property(c => c.RulingsUri).HasMaxLength(500);
        builder.Property(c => c.SetUri).HasMaxLength(500);
        builder.Property(c => c.SetSearchUri).HasMaxLength(500);
        builder.Property(c => c.ScryfallSetUri).HasMaxLength(500);

        // Decimal precision for CMC
        builder.Property(c => c.Cmc).HasPrecision(5, 2);

        // JSON columns for complex objects using EF Core 8 JSON support
        builder.OwnsOne(c => c.ImageUris, img =>
        {
            img.ToJson();
        });

        builder.OwnsOne(c => c.Prices, p =>
        {
            p.ToJson();
        });

        builder.OwnsOne(c => c.Legalities, l =>
        {
            l.ToJson();
        });

        builder.OwnsOne(c => c.Preview, p =>
        {
            p.ToJson();
        });

        // Store list properties as JSON
        builder.Property(c => c.Colors)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(c => c.ColorIdentity)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(c => c.ColorIndicator)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(c => c.ProducedMana)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(c => c.Keywords)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(c => c.Finishes)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(c => c.Games)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(c => c.FrameEffects)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(c => c.PromoTypes)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        // Store Guid lists as comma-separated strings
        builder.Property(c => c.MultiverseIds)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());

        builder.Property(c => c.ArtistIds)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList());

        builder.Property(c => c.AttractionLights)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList());

        // Store Dictionary as JSON
        builder.Property(c => c.PurchaseUris)
            .HasColumnType("nvarchar(max)")
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions?)null));

        builder.Property(c => c.RelatedUris)
            .HasColumnType("nvarchar(max)")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new());

        // Indexes for common queries
        builder.HasIndex(c => c.ScryfallId).IsUnique();
        builder.HasIndex(c => c.OracleId);
        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.SetCode);
        builder.HasIndex(c => c.Rarity);
        builder.HasIndex(c => c.Cmc);
        builder.HasIndex(c => c.TypeLine);
        builder.HasIndex(c => c.ReleasedAt);

        // Relationships
        builder.HasMany(c => c.CardFaces)
            .WithOne(cf => cf.Card)
            .HasForeignKey(cf => cf.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.AllParts)
            .WithOne(rp => rp.Card)
            .HasForeignKey(rp => rp.CardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
