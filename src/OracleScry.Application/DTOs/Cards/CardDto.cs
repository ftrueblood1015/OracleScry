namespace OracleScry.Application.DTOs.Cards;

/// <summary>
/// Full card DTO with all Scryfall data for detail views.
/// </summary>
public record CardDto(
    Guid Id,
    Guid ScryfallId,
    Guid? OracleId,

    // Core fields
    string Name,
    string Lang,
    string Layout,
    string? ManaCost,
    decimal Cmc,
    string TypeLine,
    string? OracleText,
    string? Power,
    string? Toughness,
    string? Loyalty,
    string? Defense,

    // Colors
    List<string> Colors,
    List<string> ColorIdentity,
    List<string> Keywords,

    // Set information
    string SetCode,
    string SetName,
    string SetType,
    string CollectorNumber,
    string Rarity,
    DateTime ReleasedAt,
    bool Reprint,
    bool Digital,

    // Art and print
    string? Artist,
    string? FlavorText,
    string BorderColor,
    string Frame,
    bool FullArt,
    bool Textless,
    bool Promo,
    bool Reserved,

    // Complex objects
    CardImageUrisDto? ImageUris,
    CardPricesDto Prices,
    CardLegalitiesDto Legalities,

    // Related entities
    List<CardFaceDto>? CardFaces,
    List<RelatedCardDto>? AllParts,

    // URIs
    string ScryfallUri,
    string RulingsUri,

    // Timestamps
    DateTime ImportedOn,
    DateTime LastUpdatedOn
);
