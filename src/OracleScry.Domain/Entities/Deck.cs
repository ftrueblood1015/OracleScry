using OracleScry.Domain.Identity;

namespace OracleScry.Domain.Entities;

/// <summary>
/// Represents a user's deck containing a collection of cards.
/// </summary>
public class Deck : BaseEntity
{
    /// <summary>Foreign key to the owning user</summary>
    public Guid UserId { get; set; }

    /// <summary>Deck name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional description of the deck</summary>
    public string? Description { get; set; }

    /// <summary>Optional format (standard, commander, modern, etc.)</summary>
    public string? Format { get; set; }

    /// <summary>Whether the deck is publicly visible</summary>
    public bool IsPublic { get; set; } = false;

    // Navigation properties

    /// <summary>The user who owns this deck</summary>
    public ApplicationUser User { get; set; } = null!;

    /// <summary>Cards in this deck</summary>
    public ICollection<DeckCard> DeckCards { get; set; } = [];
}
