using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for CardImportError entity.
/// </summary>
public class CardImportErrorConfiguration : IEntityTypeConfiguration<CardImportError>
{
    public void Configure(EntityTypeBuilder<CardImportError> builder)
    {
        builder.HasKey(e => e.Id);

        // String properties
        builder.Property(e => e.CardName).HasMaxLength(300);
        builder.Property(e => e.ErrorMessage).HasMaxLength(2000).IsRequired();
        builder.Property(e => e.StackTrace).HasMaxLength(4000);

        // Index for looking up errors by oracle id
        builder.HasIndex(e => e.OracleId);
        builder.HasIndex(e => e.CardImportId);
    }
}
