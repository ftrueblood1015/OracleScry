namespace OracleScry.Application.DTOs.Cards;

/// <summary>
/// DTO for related card data.
/// </summary>
public record RelatedCardDto(
    Guid Id,
    Guid RelatedCardScryfallId,
    string Component,
    string Name,
    string TypeLine,
    string Uri
);
