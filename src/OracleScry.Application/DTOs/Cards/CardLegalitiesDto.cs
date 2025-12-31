namespace OracleScry.Application.DTOs.Cards;

/// <summary>
/// DTO for card legalities across formats.
/// </summary>
public record CardLegalitiesDto(
    string Standard,
    string Future,
    string Historic,
    string Timeless,
    string Gladiator,
    string Pioneer,
    string Explorer,
    string Modern,
    string Legacy,
    string Pauper,
    string Vintage,
    string Penny,
    string Commander,
    string Oathbreaker,
    string StandardBrawl,
    string Brawl,
    string Alchemy,
    string PauperCommander,
    string Duel,
    string Oldschool,
    string Premodern,
    string Predh
);
