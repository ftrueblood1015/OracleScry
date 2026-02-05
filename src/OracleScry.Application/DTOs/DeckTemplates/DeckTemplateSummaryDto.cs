namespace OracleScry.Application.DTOs.DeckTemplates;

/// <summary>
/// Lightweight DTO for deck template list views.
/// </summary>
public record DeckTemplateSummaryDto(
    Guid Id,
    string Name,
    string? Description,
    string? Format,
    string? SetName,
    int MainboardCount,
    int SideboardCount,
    string? PreviewImageUrl,
    DateTime? ReleasedAt
);
