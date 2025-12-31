using Microsoft.EntityFrameworkCore;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;
using OracleScry.Domain.Interfaces;

namespace OracleScry.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for CardImport with specialized queries for history and statistics.
/// </summary>
public class CardImportRepository(OracleScryDbContext context) : Repository<CardImport>(context), ICardImportRepository
{
    public async Task<IReadOnlyList<CardImport>> GetHistoryAsync(int page, int pageSize, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .OrderByDescending(ci => ci.StartedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> GetCountAsync(CancellationToken ct = default)
        => await _dbSet.CountAsync(ct);

    public async Task<CardImport?> GetLatestAsync(CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .OrderByDescending(ci => ci.StartedAt)
            .FirstOrDefaultAsync(ct);

    public async Task<CardImport?> GetByIdWithErrorsAsync(Guid id, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Include(ci => ci.Errors)
            .FirstOrDefaultAsync(ci => ci.Id == id, ct);

    public async Task<IReadOnlyList<CardImport>> GetByStatusAsync(CardImportStatus status, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Where(ci => ci.Status == status)
            .OrderByDescending(ci => ci.StartedAt)
            .ToListAsync(ct);

    public async Task<bool> HasRunningImportAsync(CancellationToken ct = default)
        => await _dbSet.AnyAsync(ci =>
            ci.Status == CardImportStatus.Pending ||
            ci.Status == CardImportStatus.Downloading ||
            ci.Status == CardImportStatus.Processing, ct);

    public async Task<(int totalImports, int successful, int failed, int totalAdded, int totalUpdated)> GetAggregatedStatsAsync(CancellationToken ct = default)
    {
        var stats = await _dbSet
            .GroupBy(_ => 1)
            .Select(g => new
            {
                TotalImports = g.Count(),
                Successful = g.Count(ci => ci.Status == CardImportStatus.Completed),
                Failed = g.Count(ci => ci.Status == CardImportStatus.Failed),
                TotalAdded = g.Sum(ci => ci.CardsAdded),
                TotalUpdated = g.Sum(ci => ci.CardsUpdated)
            })
            .FirstOrDefaultAsync(ct);

        return stats != null
            ? (stats.TotalImports, stats.Successful, stats.Failed, stats.TotalAdded, stats.TotalUpdated)
            : (0, 0, 0, 0, 0);
    }
}
