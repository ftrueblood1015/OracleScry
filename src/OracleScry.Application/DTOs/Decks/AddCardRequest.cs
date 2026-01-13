namespace OracleScry.Application.DTOs.Decks;

/// <summary>
/// Request DTO for adding a card to a deck.
/// </summary>
public record AddCardRequest(
    Guid CardId,
    int Quantity = 1,
    bool IsSideboard = false
);
