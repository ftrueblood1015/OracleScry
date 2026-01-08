namespace OracleScry.Application.DTOs.Purposes;

/// <summary>
/// Full DTO for purpose extraction job with all details.
/// </summary>
public record PurposeExtractionJobDto(
    Guid Id,
    DateTime StartedAt,
    DateTime? CompletedAt,
    string Status,
    int TotalCards,
    int ProcessedCards,
    int PurposesAssigned,
    int ErrorCount,
    string? ErrorMessage,
    bool ReprocessAll,
    double? DurationSeconds
);
