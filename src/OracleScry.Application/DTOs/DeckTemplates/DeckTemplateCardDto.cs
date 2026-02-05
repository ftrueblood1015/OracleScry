namespace OracleScry.Application.DTOs.DeckTemplates;

/// <summary>
/// DTO for a card within a deck template.
/// </summary>
public record DeckTemplateCardDto(
    Guid CardId,
    string Name,
    string? ManaCost,
    decimal Cmc,
    string? TypeLine,
    string? Rarity,
    string? ImageUrl,
    int Quantity,
    bool IsSideboard,
    bool IsCommander
);
