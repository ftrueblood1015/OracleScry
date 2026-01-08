using Microsoft.EntityFrameworkCore;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;
using OracleScry.Domain.Interfaces;

namespace OracleScry.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for CardPurpose with specialized queries for purpose lookup and extraction.
/// </summary>
public class CardPurposeRepository(OracleScryDbContext context) : Repository<CardPurpose>(context), ICardPurposeRepository
{
    public async Task<CardPurpose?> GetBySlugAsync(string slug, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(cp => cp.Slug == slug, ct);

    public async Task<IReadOnlyList<CardPurpose>> GetAllActiveAsync(CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Where(cp => cp.IsActive)
            .OrderBy(cp => cp.DisplayOrder)
            .ThenBy(cp => cp.Name)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<CardPurpose>> GetByCategoryAsync(PurposeCategory category, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Where(cp => cp.Category == category && cp.IsActive)
            .OrderBy(cp => cp.DisplayOrder)
            .ThenBy(cp => cp.Name)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<CardPurpose>> GetWithPatternsAsync(CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Where(cp => cp.IsActive && !string.IsNullOrEmpty(cp.Patterns))
            .OrderBy(cp => cp.DisplayOrder)
            .ToListAsync(ct);
}
