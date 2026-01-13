namespace OracleScry.Application.DTOs.Decks;

/// <summary>
/// Request DTO for creating a new deck.
/// </summary>
public record CreateDeckRequest(
    string Name,
    string? Description,
    string? Format
);
