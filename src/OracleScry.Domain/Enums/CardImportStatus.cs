namespace OracleScry.Domain.Enums;

/// <summary>
/// Status of a card import job.
/// </summary>
public enum CardImportStatus
{
    /// <summary>Job queued, waiting to start</summary>
    Pending,

    /// <summary>Fetching bulk data file from Scryfall</summary>
    Downloading,

    /// <summary>Parsing and importing cards into database</summary>
    Processing,

    /// <summary>Successfully finished import</summary>
    Completed,

    /// <summary>Import failed with error</summary>
    Failed,

    /// <summary>Job was cancelled</summary>
    Cancelled
}
