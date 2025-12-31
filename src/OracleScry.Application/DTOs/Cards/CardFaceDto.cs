namespace OracleScry.Application.DTOs.Cards;

/// <summary>
/// DTO for card face data.
/// </summary>
public record CardFaceDto(
    Guid Id,
    string Name,
    string ManaCost,
    string? TypeLine,
    string? OracleText,
    List<string>? Colors,
    string? Power,
    string? Toughness,
    string? Loyalty,
    string? Defense,
    string? FlavorText,
    CardImageUrisDto? ImageUris,
    string? Artist,
    int FaceIndex
);
