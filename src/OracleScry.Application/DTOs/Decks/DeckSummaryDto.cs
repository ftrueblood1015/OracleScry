namespace OracleScry.Application.DTOs.Decks;

/// <summary>
/// Lightweight DTO for deck list views.
/// </summary>
public record DeckSummaryDto(
    Guid Id,
    string Name,
    string? Description,
    string? Format,
    int MainboardCount,
    int SideboardCount,
    string? PreviewImageUrl,
    DateTime UpdatedAt
);
