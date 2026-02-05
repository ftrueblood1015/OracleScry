namespace OracleScry.Application.DTOs.DeckTemplates;

/// <summary>
/// Full DTO for deck template with all cards.
/// </summary>
public record DeckTemplateDto(
    Guid Id,
    string Name,
    string? Description,
    string? Format,
    string? SetCode,
    string? SetName,
    DateTime? ReleasedAt,
    int MainboardCount,
    int SideboardCount,
    IEnumerable<DeckTemplateCardDto> Cards
);
