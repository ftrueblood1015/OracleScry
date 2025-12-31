namespace OracleScry.Domain.ValueObjects;

/// <summary>
/// Value object representing card prices from Scryfall.
/// Prices are stored as strings per Scryfall API (null if unavailable).
/// Stored as JSON column in the database.
/// </summary>
public class CardPrices
{
    public string? Usd { get; set; }
    public string? UsdFoil { get; set; }
    public string? UsdEtched { get; set; }
    public string? Eur { get; set; }
    public string? EurFoil { get; set; }
    public string? EurEtched { get; set; }
    public string? Tix { get; set; }
}
