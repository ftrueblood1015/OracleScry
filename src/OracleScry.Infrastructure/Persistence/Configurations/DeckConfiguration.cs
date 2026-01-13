using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OracleScry.Domain.Entities;

namespace OracleScry.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for Deck entity.
/// </summary>
public class DeckConfiguration : IEntityTypeConfiguration<Deck>
{
    public void Configure(EntityTypeBuilder<Deck> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.Format)
            .HasMaxLength(50);

        builder.Property(d => d.IsPublic)
            .HasDefaultValue(false);

        // Relationship to User
        builder.HasOne(d => d.User)
            .WithMany(u => u.Decks)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for efficient querying
        builder.HasIndex(d => d.UserId);
        builder.HasIndex(d => d.IsPublic);
        builder.HasIndex(d => d.Format);
    }
}
