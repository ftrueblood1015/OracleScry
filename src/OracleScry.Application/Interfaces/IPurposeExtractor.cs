using OracleScry.Domain.Entities;

namespace OracleScry.Application.Interfaces;

/// <summary>
/// Interface for extracting purposes from card oracle text.
/// </summary>
public interface IPurposeExtractor
{
    /// <summary>
    /// Extract purposes from a card's oracle text.
    /// </summary>
    /// <param name="card">The card to analyze</param>
    /// <param name="purposes">Available purposes with their patterns</param>
    /// <returns>List of matched purposes with confidence scores</returns>
    IReadOnlyList<PurposeMatch> ExtractPurposes(Card card, IReadOnlyList<CardPurpose> purposes);
}

/// <summary>
/// Result of a purpose extraction match.
/// </summary>
public record PurposeMatch(
    CardPurpose Purpose,
    decimal Confidence,
    string MatchedPattern
);
