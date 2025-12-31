using OracleScry.Domain.Enums;

namespace OracleScry.Domain.Entities;

/// <summary>
/// Tracks a single import run from Scryfall bulk data.
/// Records statistics about cards added, updated, and failed.
/// </summary>
public class CardImport : BaseEntity
{
    /// <summary>When the import job started</summary>
    public DateTime StartedAt { get; set; }

    /// <summary>When the import job completed (null if still running)</summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>Current status of the import</summary>
    public CardImportStatus Status { get; set; }

    // Statistics

    /// <summary>Total number of cards in the bulk data file</summary>
    public int TotalCardsInFile { get; set; }

    /// <summary>Number of cards processed so far</summary>
    public int CardsProcessed { get; set; }

    /// <summary>Number of new cards added to database</summary>
    public int CardsAdded { get; set; }

    /// <summary>Number of existing cards updated</summary>
    public int CardsUpdated { get; set; }

    /// <summary>Number of cards skipped (unchanged)</summary>
    public int CardsSkipped { get; set; }

    /// <summary>Number of cards that failed to import</summary>
    public int CardsFailed { get; set; }

    // Scryfall Metadata

    /// <summary>Scryfall bulk data UUID</summary>
    public string BulkDataId { get; set; } = string.Empty;

    /// <summary>Download URI used to fetch bulk data</summary>
    public string DownloadUri { get; set; } = string.Empty;

    /// <summary>When Scryfall last updated this bulk data</summary>
    public DateTime ScryfallUpdatedAt { get; set; }

    /// <summary>Size of the bulk data file in bytes</summary>
    public long FileSizeBytes { get; set; }

    // Error Info

    /// <summary>Top-level error message if import failed</summary>
    public string? ErrorMessage { get; set; }

    // Navigation

    /// <summary>Individual card errors during this import</summary>
    public ICollection<CardImportError> Errors { get; set; } = [];
}
