namespace OracleScry.Domain.Entities;

/// <summary>
/// Junction table linking Cards to their assigned Purposes.
/// Supports multiple purposes per card with confidence scores.
/// </summary>
public class CardCardPurpose
{
    /// <summary>Foreign key to Card</summary>
    public Guid CardId { get; set; }

    /// <summary>Navigation to Card</summary>
    public Card Card { get; set; } = null!;

    /// <summary>Foreign key to CardPurpose</summary>
    public Guid CardPurposeId { get; set; }

    /// <summary>Navigation to CardPurpose</summary>
    public CardPurpose CardPurpose { get; set; } = null!;

    /// <summary>Confidence score (0.0 - 1.0) of the match</summary>
    public decimal Confidence { get; set; }

    /// <summary>The regex pattern that matched (for debugging)</summary>
    public string? MatchedPattern { get; set; }

    /// <summary>When this purpose was assigned</summary>
    public DateTime AssignedAt { get; set; }

    /// <summary>What assigned this (e.g., "PatternMatcher v1.0", "Manual")</summary>
    public string AssignedBy { get; set; } = string.Empty;
}
