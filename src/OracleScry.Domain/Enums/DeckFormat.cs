namespace OracleScry.Domain.Enums;

/// <summary>
/// Supported deck formats with their specific rules.
/// </summary>
public enum DeckFormat
{
    /// <summary>No format restrictions</summary>
    Freeform,

    /// <summary>60+ cards, 4-of limit, rotating sets</summary>
    Standard,

    /// <summary>60+ cards, 4-of limit, Return to Ravnica forward</summary>
    Pioneer,

    /// <summary>60+ cards, 4-of limit, 8th Edition forward</summary>
    Modern,

    /// <summary>60+ cards, 4-of limit, all sets</summary>
    Legacy,

    /// <summary>60+ cards, 4-of limit (some restricted), all sets</summary>
    Vintage,

    /// <summary>100 cards exactly, singleton except basics</summary>
    Commander,

    /// <summary>60+ cards, commons only</summary>
    Pauper,

    /// <summary>60 cards, singleton, Standard-legal commander</summary>
    Brawl
}
