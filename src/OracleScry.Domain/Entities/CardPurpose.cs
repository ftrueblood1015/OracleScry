using OracleScry.Domain.Enums;

namespace OracleScry.Domain.Entities;

/// <summary>
/// Represents a card purpose/function category (e.g., "Creature Removal", "Card Draw").
/// This is a lookup table for the types of purposes cards can have.
/// </summary>
public class CardPurpose : BaseEntity
{
    /// <summary>Display name (e.g., "Creature Removal")</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>URL-friendly slug (e.g., "creature-removal")</summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>Description of what this purpose means</summary>
    public string? Description { get; set; }

    /// <summary>High-level category grouping</summary>
    public PurposeCategory Category { get; set; }

    /// <summary>Order for display in UI</summary>
    public int DisplayOrder { get; set; }

    /// <summary>Whether this purpose is active (soft delete)</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Regex patterns used to match this purpose (pipe-separated)</summary>
    public string? Patterns { get; set; }

    // Navigation property
    public ICollection<CardCardPurpose> CardPurposes { get; set; } = [];
}
