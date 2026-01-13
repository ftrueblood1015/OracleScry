namespace OracleScry.Application.DTOs.Decks;

/// <summary>
/// Request DTO for updating a card in a deck.
/// </summary>
public record UpdateCardRequest(
    int? Quantity,
    bool? IsSideboard
);
