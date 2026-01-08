namespace OracleScry.Application.DTOs.Purposes;

/// <summary>
/// Lightweight DTO for job list display.
/// </summary>
public record PurposeExtractionJobSummaryDto(
    Guid Id,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string Status,
    int ProcessedCards,
    int PurposesAssigned
);
