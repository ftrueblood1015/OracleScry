using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Identity;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ApplicationUser entity.
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.DisplayName).HasMaxLength(100);

        // Index on DisplayName for lookups
        builder.HasIndex(u => u.DisplayName);
    }
}
