namespace OracleScry.Application.DTOs.Purposes;

/// <summary>
/// Request DTO for updating an existing card purpose.
/// </summary>
public record UpdateCardPurposeRequest(
    string? Name,
    string? Description,
    string? Category,
    int? DisplayOrder,
    bool? IsActive,
    string? Patterns
);
