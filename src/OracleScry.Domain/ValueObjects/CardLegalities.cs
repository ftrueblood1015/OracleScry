namespace OracleScry.Domain.ValueObjects;

/// <summary>
/// Value object representing card legalities across all MTG formats.
/// Values are: "legal", "not_legal", "restricted", "banned".
/// Stored as JSON column in the database.
/// </summary>
public class CardLegalities
{
    public string Standard { get; set; } = "not_legal";
    public string Future { get; set; } = "not_legal";
    public string Historic { get; set; } = "not_legal";
    public string Timeless { get; set; } = "not_legal";
    public string Gladiator { get; set; } = "not_legal";
    public string Pioneer { get; set; } = "not_legal";
    public string Explorer { get; set; } = "not_legal";
    public string Modern { get; set; } = "not_legal";
    public string Legacy { get; set; } = "not_legal";
    public string Pauper { get; set; } = "not_legal";
    public string Vintage { get; set; } = "not_legal";
    public string Penny { get; set; } = "not_legal";
    public string Commander { get; set; } = "not_legal";
    public string Oathbreaker { get; set; } = "not_legal";
    public string StandardBrawl { get; set; } = "not_legal";
    public string Brawl { get; set; } = "not_legal";
    public string Alchemy { get; set; } = "not_legal";
    public string PauperCommander { get; set; } = "not_legal";
    public string Duel { get; set; } = "not_legal";
    public string Oldschool { get; set; } = "not_legal";
    public string Premodern { get; set; } = "not_legal";
    public string Predh { get; set; } = "not_legal";
}
