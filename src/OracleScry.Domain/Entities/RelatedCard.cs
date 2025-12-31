namespace OracleScry.Domain.Entities;

/// <summary>
/// Related card entity for cards that are connected (tokens, meld pairs, etc.).
/// Links cards through Scryfall's all_parts relationship.
/// </summary>
public class RelatedCard
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;

    public string Object { get; set; } = "related_card";
    public Guid RelatedCardScryfallId { get; set; }
    public string Component { get; set; } = string.Empty; // token, meld_part, meld_result, combo_piece
    public string Name { get; set; } = string.Empty;
    public string TypeLine { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
}
