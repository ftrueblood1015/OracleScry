namespace OracleScry.Application.DTOs.Purposes;

/// <summary>
/// Lightweight DTO for purpose display (without patterns).
/// </summary>
public record CardPurposeSummaryDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string Category
);
