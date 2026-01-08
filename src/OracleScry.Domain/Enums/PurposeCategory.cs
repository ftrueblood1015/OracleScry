namespace OracleScry.Domain.Enums;

/// <summary>
/// Categories for card purposes/functions.
/// </summary>
public enum PurposeCategory
{
    /// <summary>Creature/Artifact/Enchantment/Planeswalker destruction or exile</summary>
    Removal,

    /// <summary>Draw cards, tutor, scry, filtering</summary>
    CardAdvantage,

    /// <summary>Mana acceleration, land fetching</summary>
    Ramp,

    /// <summary>Counter spells and abilities</summary>
    Counterspell,

    /// <summary>Combat tricks, pump spells, evasion</summary>
    Combat,

    /// <summary>Hexproof, indestructible, regenerate, ward</summary>
    Protection,

    /// <summary>Graveyard retrieval, reanimation</summary>
    Recursion,

    /// <summary>Token generation</summary>
    Tokens,

    /// <summary>Life gain effects</summary>
    LifeGain,

    /// <summary>Direct damage to creatures/players</summary>
    Damage,

    /// <summary>Library destruction (mill)</summary>
    Mill,

    /// <summary>Hand disruption, forced discard</summary>
    Discard,

    /// <summary>Catch-all for other effects</summary>
    Other
}
