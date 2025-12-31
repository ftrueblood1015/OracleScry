namespace OracleScry.Application.DTOs.Import;

/// <summary>
/// Aggregated import statistics.
/// </summary>
public record ImportStatsDto(
    int TotalImports,
    int SuccessfulImports,
    int FailedImports,
    int TotalCardsAdded,
    int TotalCardsUpdated,
    DateTime? LastImportAt,
    double? AverageDurationSeconds
);
