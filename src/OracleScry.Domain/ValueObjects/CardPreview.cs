namespace OracleScry.Domain.ValueObjects;

/// <summary>
/// Value object representing card preview information from Scryfall.
/// Contains when and where a card was first previewed.
/// Stored as JSON column in the database.
/// </summary>
public class CardPreview
{
    public DateTime? PreviewedAt { get; set; }
    public string? SourceUri { get; set; }
    public string? Source { get; set; }
}
