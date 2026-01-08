namespace OracleScry.Application.DTOs.Purposes;

/// <summary>
/// DTO for a purpose assigned to a card (from junction table).
/// </summary>
public record CardPurposeAssignmentDto(
    Guid PurposeId,
    string Name,
    string Slug,
    string Category,
    decimal Confidence,
    string? MatchedPattern,
    DateTime AssignedAt,
    string AssignedBy
);
