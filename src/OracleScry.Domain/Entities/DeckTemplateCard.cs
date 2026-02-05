namespace OracleScry.Domain.Entities;

/// <summary>
/// Junction entity representing a card in a deck template with quantity and board placement.
/// </summary>
public class DeckTemplateCard
{
    /// <summary>Foreign key to the deck template</summary>
    public Guid DeckTemplateId { get; set; }

    /// <summary>Foreign key to the card</summary>
    public Guid CardId { get; set; }

    /// <summary>Number of copies in the template</summary>
    public int Quantity { get; set; } = 1;

    /// <summary>Whether this card is in the sideboard</summary>
    public bool IsSideboard { get; set; } = false;

    /// <summary>Whether this card is the commander (Commander format only)</summary>
    public bool IsCommander { get; set; } = false;

    // Navigation properties

    /// <summary>The deck template containing this card</summary>
    public DeckTemplate DeckTemplate { get; set; } = null!;

    /// <summary>The card in the template</summary>
    public Card Card { get; set; } = null!;
}
