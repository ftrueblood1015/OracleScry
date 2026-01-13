namespace OracleScry.Application.DTOs.Decks;

/// <summary>
/// Deck statistics including mana curve, colors, types, and purposes.
/// </summary>
public record DeckStatsDto(
    int TotalCards,
    int MainboardCount,
    int SideboardCount,
    int UniqueCards,
    Dictionary<int, int> ManaCurve,
    Dictionary<string, int> ColorDistribution,
    Dictionary<string, int> TypeDistribution,
    Dictionary<string, int> PurposeBreakdown,
    Dictionary<string, int> RarityDistribution,
    decimal AverageCmc,
    decimal? EstimatedPrice
);
