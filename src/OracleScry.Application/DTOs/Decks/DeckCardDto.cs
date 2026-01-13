namespace OracleScry.Application.DTOs.Decks;

/// <summary>
/// DTO for a card within a deck, including quantity and purposes.
/// </summary>
public record DeckCardDto(
    Guid CardId,
    string Name,
    string? ManaCost,
    decimal Cmc,
    string TypeLine,
    string? Rarity,
    string? ImageUrl,
    int Quantity,
    bool IsSideboard,
    List<string> Purposes
);
