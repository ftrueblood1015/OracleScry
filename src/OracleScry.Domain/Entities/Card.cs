using OracleScry.Domain.ValueObjects;

namespace OracleScry.Domain.Entities;

/// <summary>
/// Card entity representing an MTG card with full Scryfall schema.
/// This is the aggregate root for card-related data.
/// </summary>
public class Card : BaseEntity
{
    // Scryfall Core Fields
    public Guid ScryfallId { get; set; }
    public string Object { get; set; } = "card";
    public string Uri { get; set; } = string.Empty;
    public string ScryfallUri { get; set; } = string.Empty;
    public string PrintsSearchUri { get; set; } = string.Empty;
    public string RulingsUri { get; set; } = string.Empty;

    // Identification Fields
    public int? ArenaId { get; set; }
    public int? MtgoId { get; set; }
    public int? MtgoFoilId { get; set; }
    public List<int> MultiverseIds { get; set; } = [];
    public int? TcgplayerId { get; set; }
    public int? TcgplayerEtchedId { get; set; }
    public int? CardmarketId { get; set; }
    public Guid? OracleId { get; set; }

    // Gameplay Fields
    public string Name { get; set; } = string.Empty;
    public string Lang { get; set; } = "en";
    public string Layout { get; set; } = string.Empty;
    public string? ManaCost { get; set; }
    public decimal Cmc { get; set; }
    public string TypeLine { get; set; } = string.Empty;
    public string? OracleText { get; set; }
    public string? Power { get; set; }
    public string? Toughness { get; set; }
    public string? Loyalty { get; set; }
    public string? Defense { get; set; }
    public List<string> Colors { get; set; } = [];
    public List<string> ColorIdentity { get; set; } = [];
    public List<string>? ColorIndicator { get; set; }
    public List<string>? ProducedMana { get; set; }
    public List<string> Keywords { get; set; } = [];
    public bool Reserved { get; set; }
    public int? EdhrecRank { get; set; }
    public int? PennyRank { get; set; }
    public bool? GameChanger { get; set; }
    public string? HandModifier { get; set; }
    public string? LifeModifier { get; set; }

    // Print Fields
    public string? Artist { get; set; }
    public List<Guid> ArtistIds { get; set; } = [];
    public bool Booster { get; set; }
    public string BorderColor { get; set; } = string.Empty;
    public Guid CardBackId { get; set; }
    public string CollectorNumber { get; set; } = string.Empty;
    public bool? ContentWarning { get; set; }
    public bool Digital { get; set; }
    public List<string> Finishes { get; set; } = [];
    public string? FlavorName { get; set; }
    public string? FlavorText { get; set; }
    public List<string>? FrameEffects { get; set; }
    public string Frame { get; set; } = string.Empty;
    public bool FullArt { get; set; }
    public List<string> Games { get; set; } = [];
    public bool HighresImage { get; set; }
    public Guid? IllustrationId { get; set; }
    public string ImageStatus { get; set; } = string.Empty;
    public bool Oversized { get; set; }
    public string? PrintedName { get; set; }
    public string? PrintedText { get; set; }
    public string? PrintedTypeLine { get; set; }
    public bool Promo { get; set; }
    public List<string>? PromoTypes { get; set; }
    public string Rarity { get; set; } = string.Empty;
    public DateTime ReleasedAt { get; set; }
    public bool Reprint { get; set; }
    public string SetCode { get; set; } = string.Empty;
    public Guid SetId { get; set; }
    public string SetName { get; set; } = string.Empty;
    public string SetType { get; set; } = string.Empty;
    public string SetUri { get; set; } = string.Empty;
    public string SetSearchUri { get; set; } = string.Empty;
    public string ScryfallSetUri { get; set; } = string.Empty;
    public bool StorySpotlight { get; set; }
    public bool Textless { get; set; }
    public bool Variation { get; set; }
    public Guid? VariationOf { get; set; }
    public string? SecurityStamp { get; set; }
    public string? Watermark { get; set; }
    public List<int>? AttractionLights { get; set; }

    // Complex Objects (JSON Columns)
    public CardImageUris? ImageUris { get; set; }
    public CardPrices Prices { get; set; } = new();
    public CardLegalities Legalities { get; set; } = new();
    public Dictionary<string, string>? PurchaseUris { get; set; }
    public Dictionary<string, string> RelatedUris { get; set; } = new();
    public CardPreview? Preview { get; set; }

    // Navigation Properties (Separate Tables)
    public ICollection<CardFace> CardFaces { get; set; } = [];
    public ICollection<RelatedCard> AllParts { get; set; } = [];
}
