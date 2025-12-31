using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for CardImport entity.
/// </summary>
public class CardImportConfiguration : IEntityTypeConfiguration<CardImport>
{
    public void Configure(EntityTypeBuilder<CardImport> builder)
    {
        builder.HasKey(ci => ci.Id);

        // String properties
        builder.Property(ci => ci.BulkDataId).HasMaxLength(100).IsRequired();
        builder.Property(ci => ci.DownloadUri).HasMaxLength(500).IsRequired();
        builder.Property(ci => ci.ErrorMessage).HasMaxLength(2000);

        // Status as string for readability
        builder.Property(ci => ci.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Indexes
        builder.HasIndex(ci => ci.StartedAt).IsDescending();
        builder.HasIndex(ci => ci.Status);
        builder.HasIndex(ci => ci.ScryfallUpdatedAt);

        // Relationships
        builder.HasMany(ci => ci.Errors)
            .WithOne(e => e.CardImport)
            .HasForeignKey(e => e.CardImportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
