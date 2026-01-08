using Microsoft.EntityFrameworkCore;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;
using OracleScry.Domain.Interfaces;

namespace OracleScry.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for PurposeExtractionJob with specialized queries for job history and status.
/// </summary>
public class PurposeExtractionJobRepository(OracleScryDbContext context) : Repository<PurposeExtractionJob>(context), IPurposeExtractionJobRepository
{
    public async Task<IReadOnlyList<PurposeExtractionJob>> GetHistoryAsync(int page, int pageSize, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .OrderByDescending(pej => pej.StartedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<int> GetCountAsync(CancellationToken ct = default)
        => await _dbSet.CountAsync(ct);

    public async Task<PurposeExtractionJob?> GetLatestAsync(CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .OrderByDescending(pej => pej.StartedAt)
            .FirstOrDefaultAsync(ct);

    public async Task<bool> HasRunningJobAsync(CancellationToken ct = default)
        => await _dbSet.AnyAsync(pej =>
            pej.Status == ExtractionJobStatus.Pending ||
            pej.Status == ExtractionJobStatus.Running, ct);

    public async Task<IReadOnlyList<PurposeExtractionJob>> GetByStatusAsync(ExtractionJobStatus status, CancellationToken ct = default)
        => await _dbSet
            .AsNoTracking()
            .Where(pej => pej.Status == status)
            .OrderByDescending(pej => pej.StartedAt)
            .ToListAsync(ct);
}
