namespace OracleScry.Domain.Entities;

/// <summary>
/// Represents a preconstructed deck template that users can copy to create their own decks.
/// </summary>
public class DeckTemplate : BaseEntity
{
    /// <summary>Template name (e.g., "Bloomburrow Commander - Animated Army")</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional description of the deck template</summary>
    public string? Description { get; set; }

    /// <summary>Format (Commander, Standard, Modern, etc.)</summary>
    public string? Format { get; set; }

    /// <summary>Scryfall set code (e.g., "blc" for Bloomburrow Commander)</summary>
    public string? SetCode { get; set; }

    /// <summary>Set name (e.g., "Bloomburrow Commander")</summary>
    public string? SetName { get; set; }

    /// <summary>External reference to Scryfall deck ID</summary>
    public string? ScryfallDeckId { get; set; }

    /// <summary>When the preconstructed deck was released</summary>
    public DateTime? ReleasedAt { get; set; }

    /// <summary>Whether this template is active/visible (soft delete)</summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>Cards in this template</summary>
    public ICollection<DeckTemplateCard> TemplateCards { get; set; } = [];
}
