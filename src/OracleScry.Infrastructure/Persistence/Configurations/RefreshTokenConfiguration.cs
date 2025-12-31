using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for RefreshToken entity.
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Token).HasMaxLength(500).IsRequired();
        builder.Property(rt => rt.ReplacedByToken).HasMaxLength(500);

        // Index on Token for lookups during refresh
        builder.HasIndex(rt => rt.Token);
        builder.HasIndex(rt => rt.UserId);

        // Relationship to ApplicationUser
        builder.HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore computed properties
        builder.Ignore(rt => rt.IsExpired);
        builder.Ignore(rt => rt.IsRevoked);
        builder.Ignore(rt => rt.IsActive);
    }
}
