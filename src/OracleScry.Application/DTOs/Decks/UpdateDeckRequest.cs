namespace OracleScry.Application.DTOs.Decks;

/// <summary>
/// Request DTO for updating a deck.
/// </summary>
public record UpdateDeckRequest(
    string? Name,
    string? Description,
    string? Format,
    bool? IsPublic
);
