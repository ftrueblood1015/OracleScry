using Microsoft.EntityFrameworkCore;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Interfaces;

namespace OracleScry.Infrastructure.Persistence.Repositories;

/// <summary>
/// Card-specific repository with optimized queries for card operations.
/// </summary>
public class CardRepository(OracleScryDbContext context) : Repository<Card>(context), ICardRepository
{
    public async Task<Card?> GetByScryfallIdAsync(Guid scryfallId, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ScryfallId == scryfallId, ct);

    public async Task<Card?> GetByIdWithFacesAsync(Guid id, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Include(c => c.CardFaces.OrderBy(cf => cf.FaceIndex))
            .Include(c => c.AllParts)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<Card>> GetBySetCodeAsync(string setCode, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Where(c => c.SetCode == setCode.ToLowerInvariant())
            .OrderBy(c => c.CollectorNumber)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<Card>> GetByOracleIdAsync(Guid oracleId, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Where(c => c.OracleId == oracleId)
            .OrderByDescending(c => c.ReleasedAt)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<string>> GetDistinctSetCodesAsync(CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Select(c => c.SetCode)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync(ct);

    public async Task<int> GetCountAsync(CancellationToken ct = default)
        => await _dbSet.CountAsync(ct);

    public async Task<IReadOnlyList<Card>> GetRandomAsync(int count, CancellationToken ct = default)
    {
        // Use SQL Server's NEWID() for random ordering
        return await _dbSet
            .AsNoTracking()
            .OrderBy(c => Guid.NewGuid())
            .Take(count)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistsByScryfallIdAsync(Guid scryfallId, CancellationToken ct = default)
        => await _dbSet.AnyAsync(c => c.ScryfallId == scryfallId, ct);
}
