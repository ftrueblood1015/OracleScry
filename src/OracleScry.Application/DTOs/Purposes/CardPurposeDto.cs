namespace OracleScry.Application.DTOs.Purposes;

/// <summary>
/// DTO for CardPurpose entity with full details.
/// </summary>
public record CardPurposeDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string Category,
    int DisplayOrder,
    bool IsActive,
    string? Patterns
);
