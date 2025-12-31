namespace OracleScry.Application.DTOs.Cards;

/// <summary>
/// DTO for card prices.
/// </summary>
public record CardPricesDto(
    string? Usd,
    string? UsdFoil,
    string? UsdEtched,
    string? Eur,
    string? EurFoil,
    string? EurEtched,
    string? Tix
);
