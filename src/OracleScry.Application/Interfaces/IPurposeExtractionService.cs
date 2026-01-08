using OracleScry.Application.DTOs.Common;
using OracleScry.Application.DTOs.Purposes;

namespace OracleScry.Application.Interfaces;

/// <summary>
/// Service for purpose extraction operations.
/// </summary>
public interface IPurposeExtractionService
{
    /// <summary>Execute purpose extraction for all cards</summary>
    Task<PurposeExtractionJobDto> ExecuteExtractionAsync(bool reprocessAll = false, CancellationToken ct = default);

    /// <summary>Get paginated extraction job history</summary>
    Task<PagedResult<PurposeExtractionJobSummaryDto>> GetHistoryAsync(int page = 1, int pageSize = 20, CancellationToken ct = default);

    /// <summary>Get a single extraction job</summary>
    Task<PurposeExtractionJobDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Get the most recent extraction job</summary>
    Task<PurposeExtractionJobSummaryDto?> GetLatestAsync(CancellationToken ct = default);

    /// <summary>Get aggregated extraction statistics</summary>
    Task<ExtractionStatsDto> GetStatsAsync(CancellationToken ct = default);

    /// <summary>Check if an extraction is currently running</summary>
    Task<bool> IsExtractionRunningAsync(CancellationToken ct = default);
}
