namespace OracleScry.Application.DTOs.Import;

/// <summary>
/// Full DTO for card import with all details.
/// </summary>
public record CardImportDto(
    Guid Id,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string Status,

    // Statistics
    int TotalCardsInFile,
    int CardsProcessed,
    int CardsAdded,
    int CardsUpdated,
    int CardsSkipped,
    int CardsFailed,

    // Scryfall Metadata
    string BulkDataId,
    string DownloadUri,
    DateTime ScryfallUpdatedAt,
    long FileSizeBytes,

    // Error Info
    string? ErrorMessage,

    // Computed
    double? DurationSeconds,

    // Errors (only when requested)
    List<CardImportErrorDto>? Errors
);
