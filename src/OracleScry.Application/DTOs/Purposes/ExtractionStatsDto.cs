namespace OracleScry.Application.DTOs.Purposes;

/// <summary>
/// Aggregated statistics for purpose extraction.
/// </summary>
public record ExtractionStatsDto(
    int TotalJobs,
    int SuccessfulJobs,
    int FailedJobs,
    int TotalCardsProcessed,
    int TotalPurposesAssigned,
    DateTime? LastExtraction,
    int CardsWithPurposes,
    int CardsWithoutPurposes
);
