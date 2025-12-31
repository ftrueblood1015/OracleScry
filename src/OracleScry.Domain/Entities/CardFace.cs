using OracleScry.Domain.ValueObjects;

namespace OracleScry.Domain.Entities;

/// <summary>
/// Card face entity for multi-faced cards (transform, modal double-faced, etc.).
/// Represents one face of a card with its own properties.
/// </summary>
public class CardFace
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;

    public string Object { get; set; } = "card_face";
    public string Name { get; set; } = string.Empty;
    public string ManaCost { get; set; } = string.Empty;
    public string? TypeLine { get; set; }
    public string? OracleText { get; set; }
    public List<string>? Colors { get; set; }
    public List<string>? ColorIndicator { get; set; }
    public string? Power { get; set; }
    public string? Toughness { get; set; }
    public string? Loyalty { get; set; }
    public string? Defense { get; set; }
    public string? FlavorText { get; set; }
    public Guid? IllustrationId { get; set; }
    public CardImageUris? ImageUris { get; set; }
    public decimal? Cmc { get; set; }
    public Guid? OracleId { get; set; }
    public string? Layout { get; set; }
    public string? PrintedName { get; set; }
    public string? PrintedText { get; set; }
    public string? PrintedTypeLine { get; set; }
    public string? Watermark { get; set; }
    public string? Artist { get; set; }
    public Guid? ArtistId { get; set; }
    public int FaceIndex { get; set; }
}
