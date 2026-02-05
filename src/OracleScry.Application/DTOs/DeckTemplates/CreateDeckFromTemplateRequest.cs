namespace OracleScry.Application.DTOs.DeckTemplates;

/// <summary>
/// Request to create a new deck from a template.
/// </summary>
public record CreateDeckFromTemplateRequest(
    string Name,
    string? Description = null,
    bool CopyFormat = true
);
