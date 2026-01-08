using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for PurposeExtractionJob entity.
/// </summary>
public class PurposeExtractionJobConfiguration : IEntityTypeConfiguration<PurposeExtractionJob>
{
    public void Configure(EntityTypeBuilder<PurposeExtractionJob> builder)
    {
        builder.HasKey(pej => pej.Id);

        // Status as string for readability
        builder.Property(pej => pej.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(pej => pej.ErrorMessage)
            .HasMaxLength(2000);

        // Indexes
        builder.HasIndex(pej => pej.StartedAt).IsDescending();
        builder.HasIndex(pej => pej.Status);
    }
}
