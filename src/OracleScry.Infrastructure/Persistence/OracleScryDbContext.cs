using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Identity;

namespace OracleScry.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for OracleScry.
/// Extends IdentityDbContext for user authentication support.
/// </summary>
public class OracleScryDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public OracleScryDbContext(DbContextOptions<OracleScryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Card> Cards => Set<Card>();
    public DbSet<CardFace> CardFaces => Set<CardFace>();
    public DbSet<RelatedCard> RelatedCards => Set<RelatedCard>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<CardImport> CardImports => Set<CardImport>();
    public DbSet<CardImportError> CardImportErrors => Set<CardImportError>();
    public DbSet<CardPurpose> CardPurposes => Set<CardPurpose>();
    public DbSet<CardCardPurpose> CardCardPurposes => Set<CardCardPurpose>();
    public DbSet<PurposeExtractionJob> PurposeExtractionJobs => Set<PurposeExtractionJob>();
    public DbSet<Deck> Decks => Set<Deck>();
    public DbSet<DeckCard> DeckCards => Set<DeckCard>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OracleScryDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Auto-update LastUpdatedOn for modified entities
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.ImportedOn = DateTime.UtcNow;
                entry.Entity.LastUpdatedOn = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastUpdatedOn = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
