namespace OracleScry.Application.DTOs.Cards;

/// <summary>
/// Lightweight DTO for card list views.
/// Contains only essential fields for display in grids/lists.
/// </summary>
public record CardSummaryDto(
    Guid Id,
    string Name,
    string? ManaCost,
    string TypeLine,
    string Rarity,
    string SetCode,
    string SetName,
    string? ImageUri,
    string? Price
);
