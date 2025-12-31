using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;

namespace OracleScry.Domain.Interfaces;

/// <summary>
/// Repository interface for CardImport entities.
/// Provides methods for querying import history and statistics.
/// </summary>
public interface ICardImportRepository : IRepository<CardImport>
{
    /// <summary>Get paginated import history ordered by most recent first</summary>
    Task<IReadOnlyList<CardImport>> GetHistoryAsync(int page, int pageSize, CancellationToken ct = default);

    /// <summary>Get total count of imports for pagination</summary>
    Task<int> GetCountAsync(CancellationToken ct = default);

    /// <summary>Get the most recent import</summary>
    Task<CardImport?> GetLatestAsync(CancellationToken ct = default);

    /// <summary>Get import with its errors loaded</summary>
    Task<CardImport?> GetByIdWithErrorsAsync(Guid id, CancellationToken ct = default);

    /// <summary>Get imports by status</summary>
    Task<IReadOnlyList<CardImport>> GetByStatusAsync(CardImportStatus status, CancellationToken ct = default);

    /// <summary>Check if there's an import currently running</summary>
    Task<bool> HasRunningImportAsync(CancellationToken ct = default);

    /// <summary>Get aggregated statistics across all imports</summary>
    Task<(int totalImports, int successful, int failed, int totalAdded, int totalUpdated)> GetAggregatedStatsAsync(CancellationToken ct = default);
}
