namespace OracleScry.Application.DTOs.Decks;

/// <summary>
/// Full deck details with all cards.
/// </summary>
public record DeckDto(
    Guid Id,
    string Name,
    string? Description,
    string? Format,
    bool IsPublic,
    int MainboardCount,
    int SideboardCount,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<DeckCardDto> Cards
);
