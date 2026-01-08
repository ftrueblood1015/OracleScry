using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;

namespace OracleScry.Domain.Interfaces;

/// <summary>
/// Repository interface for PurposeExtractionJob entities.
/// </summary>
public interface IPurposeExtractionJobRepository : IRepository<PurposeExtractionJob>
{
    /// <summary>Get paginated job history ordered by most recent first</summary>
    Task<IReadOnlyList<PurposeExtractionJob>> GetHistoryAsync(int page, int pageSize, CancellationToken ct = default);

    /// <summary>Get total count of jobs for pagination</summary>
    Task<int> GetCountAsync(CancellationToken ct = default);

    /// <summary>Get the most recent job</summary>
    Task<PurposeExtractionJob?> GetLatestAsync(CancellationToken ct = default);

    /// <summary>Check if there's a job currently running</summary>
    Task<bool> HasRunningJobAsync(CancellationToken ct = default);

    /// <summary>Get jobs by status</summary>
    Task<IReadOnlyList<PurposeExtractionJob>> GetByStatusAsync(ExtractionJobStatus status, CancellationToken ct = default);
}
