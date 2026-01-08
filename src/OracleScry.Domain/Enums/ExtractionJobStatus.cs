namespace OracleScry.Domain.Enums;

/// <summary>
/// Status of a purpose extraction job.
/// </summary>
public enum ExtractionJobStatus
{
    /// <summary>Job queued, waiting to start</summary>
    Pending,

    /// <summary>Currently processing cards</summary>
    Running,

    /// <summary>Successfully finished extraction</summary>
    Completed,

    /// <summary>Extraction failed with error</summary>
    Failed,

    /// <summary>Job was cancelled</summary>
    Cancelled
}
