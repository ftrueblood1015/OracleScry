using OracleScry.Application.DTOs.Common;
using OracleScry.Application.DTOs.Import;

namespace OracleScry.Application.Interfaces;

/// <summary>
/// Service for card import operations.
/// </summary>
public interface ICardImportService
{
    /// <summary>Execute a full import from Scryfall bulk data</summary>
    Task<CardImportDto> ExecuteImportAsync(CancellationToken ct = default);

    /// <summary>Get paginated import history</summary>
    Task<PagedResult<CardImportSummaryDto>> GetHistoryAsync(int page = 1, int pageSize = 20, CancellationToken ct = default);

    /// <summary>Get a single import with errors</summary>
    Task<CardImportDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Get the most recent import</summary>
    Task<CardImportSummaryDto?> GetLatestAsync(CancellationToken ct = default);

    /// <summary>Get aggregated import statistics</summary>
    Task<ImportStatsDto> GetStatsAsync(CancellationToken ct = default);

    /// <summary>Check if an import is currently running</summary>
    Task<bool> IsImportRunningAsync(CancellationToken ct = default);
}
