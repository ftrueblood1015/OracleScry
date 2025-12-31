namespace OracleScry.Domain.ValueObjects;

/// <summary>
/// Value object representing card image URIs from Scryfall.
/// Stored as JSON column in the database.
/// </summary>
public class CardImageUris
{
    public string? Small { get; set; }
    public string? Normal { get; set; }
    public string? Large { get; set; }
    public string? Png { get; set; }
    public string? ArtCrop { get; set; }
    public string? BorderCrop { get; set; }
}
