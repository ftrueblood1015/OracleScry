namespace OracleScry.Application.DTOs.Import;

/// <summary>
/// Lightweight DTO for import history listing.
/// </summary>
public record CardImportSummaryDto(
    Guid Id,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string Status,
    int CardsAdded,
    int CardsUpdated,
    int CardsFailed,
    double? DurationSeconds
);
