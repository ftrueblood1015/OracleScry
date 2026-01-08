using OracleScry.Domain.Enums;

namespace OracleScry.Domain.Entities;

/// <summary>
/// Tracks the execution of purpose extraction background jobs.
/// Similar to CardImport but for purpose analysis.
/// </summary>
public class PurposeExtractionJob : BaseEntity
{
    /// <summary>When the job started processing</summary>
    public DateTime StartedAt { get; set; }

    /// <summary>When the job completed (null if still running)</summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>Current status of the job</summary>
    public ExtractionJobStatus Status { get; set; } = ExtractionJobStatus.Pending;

    /// <summary>Total number of cards to process</summary>
    public int TotalCards { get; set; }

    /// <summary>Number of cards processed so far</summary>
    public int ProcessedCards { get; set; }

    /// <summary>Number of purpose assignments created</summary>
    public int PurposesAssigned { get; set; }

    /// <summary>Number of errors encountered</summary>
    public int ErrorCount { get; set; }

    /// <summary>Error message if job failed</summary>
    public string? ErrorMessage { get; set; }

    /// <summary>Whether to reprocess all cards or just new ones</summary>
    public bool ReprocessAll { get; set; }
}
