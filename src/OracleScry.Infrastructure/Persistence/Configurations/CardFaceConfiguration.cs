using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for CardFace entity.
/// </summary>
public class CardFaceConfiguration : IEntityTypeConfiguration<CardFace>
{
    public void Configure(EntityTypeBuilder<CardFace> builder)
    {
        builder.HasKey(cf => cf.Id);

        builder.Property(cf => cf.Name).HasMaxLength(300).IsRequired();
        builder.Property(cf => cf.ManaCost).HasMaxLength(100).IsRequired();
        builder.Property(cf => cf.Object).HasMaxLength(20).IsRequired();

        builder.Property(cf => cf.TypeLine).HasMaxLength(200);
        builder.Property(cf => cf.OracleText).HasMaxLength(2000);
        builder.Property(cf => cf.Power).HasMaxLength(10);
        builder.Property(cf => cf.Toughness).HasMaxLength(10);
        builder.Property(cf => cf.Loyalty).HasMaxLength(10);
        builder.Property(cf => cf.Defense).HasMaxLength(10);
        builder.Property(cf => cf.FlavorText).HasMaxLength(1000);
        builder.Property(cf => cf.Layout).HasMaxLength(50);
        builder.Property(cf => cf.PrintedName).HasMaxLength(300);
        builder.Property(cf => cf.PrintedText).HasMaxLength(2000);
        builder.Property(cf => cf.PrintedTypeLine).HasMaxLength(200);
        builder.Property(cf => cf.Watermark).HasMaxLength(50);
        builder.Property(cf => cf.Artist).HasMaxLength(200);

        builder.Property(cf => cf.Cmc).HasPrecision(5, 2);

        // JSON column for ImageUris
        builder.OwnsOne(cf => cf.ImageUris, img =>
        {
            img.ToJson();
        });

        // Store list properties as comma-separated strings
        builder.Property(cf => cf.Colors)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(cf => cf.ColorIndicator)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        // Index on CardId for efficient joins
        builder.HasIndex(cf => cf.CardId);
        builder.HasIndex(cf => cf.Name);
    }
}
