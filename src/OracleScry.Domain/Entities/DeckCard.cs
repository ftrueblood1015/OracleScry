namespace OracleScry.Domain.Entities;

/// <summary>
/// Junction entity representing a card in a deck with quantity and board placement.
/// </summary>
public class DeckCard
{
    /// <summary>Foreign key to the deck</summary>
    public Guid DeckId { get; set; }

    /// <summary>Foreign key to the card</summary>
    public Guid CardId { get; set; }

    /// <summary>Number of copies (typically 1-4, unlimited for basics in some formats)</summary>
    public int Quantity { get; set; } = 1;

    /// <summary>Whether this card is in the sideboard</summary>
    public bool IsSideboard { get; set; } = false;

    /// <summary>When the card was added to the deck</summary>
    public DateTime AddedAt { get; set; }

    // Navigation properties

    /// <summary>The deck containing this card</summary>
    public Deck Deck { get; set; } = null!;

    /// <summary>The card in the deck</summary>
    public Card Card { get; set; } = null!;
}
