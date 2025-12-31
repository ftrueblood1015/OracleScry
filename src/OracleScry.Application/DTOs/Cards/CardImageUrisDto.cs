namespace OracleScry.Application.DTOs.Cards;

/// <summary>
/// DTO for card image URIs.
/// </summary>
public record CardImageUrisDto(
    string? Small,
    string? Normal,
    string? Large,
    string? Png,
    string? ArtCrop,
    string? BorderCrop
);
