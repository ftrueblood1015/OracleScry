namespace OracleScry.Application.DTOs.Purposes;

/// <summary>
/// Request DTO for creating a new card purpose.
/// </summary>
public record CreateCardPurposeRequest(
    string Name,
    string? Description,
    string Category,
    int DisplayOrder,
    string? Patterns
);
