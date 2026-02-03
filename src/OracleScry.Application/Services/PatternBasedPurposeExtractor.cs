using System.Text.RegularExpressions;
using OracleScry.Application.Interfaces;
using OracleScry.Domain.Entities;

namespace OracleScry.Application.Services;

/// <summary>
/// Extracts card purposes using regex pattern matching against oracle text.
/// Patterns are stored pipe-delimited in CardPurpose.Patterns field.
/// </summary>
public class PatternBasedPurposeExtractor : IPurposeExtractor
{
    private readonly Dictionary<Guid, List<Regex>> _compiledPatterns = new();

    public IReadOnlyList<PurposeMatch> ExtractPurposes(Card card, IReadOnlyList<CardPurpose> purposes)
    {
        var matches = new List<PurposeMatch>();

        // Get oracle text and type line from card or its faces
        var oracleText = GetOracleText(card);
        var typeLine = GetTypeLine(card);

        if (string.IsNullOrWhiteSpace(oracleText))
        {
            return matches;
        }

        foreach (var purpose in purposes)
        {
            if (string.IsNullOrWhiteSpace(purpose.Patterns))
                continue;

            // Check type constraints before pattern matching
            if (!MeetsTypeConstraints(typeLine, purpose))
                continue;

            var regexPatterns = GetCompiledPatterns(purpose);

            foreach (var regex in regexPatterns)
            {
                var match = regex.Match(oracleText);
                if (match.Success)
                {
                    // Calculate confidence based on match quality
                    var confidence = CalculateConfidence(match, oracleText);

                    matches.Add(new PurposeMatch(
                        purpose,
                        confidence,
                        match.Value
                    ));

                    // Only match first pattern per purpose
                    break;
                }
            }
        }

        return matches;
    }

    private static string GetOracleText(Card card)
    {
        // First try the main oracle text
        if (!string.IsNullOrWhiteSpace(card.OracleText))
        {
            return card.OracleText;
        }

        // For multi-face cards, concatenate face oracle texts
        if (card.CardFaces?.Count > 0)
        {
            var faceTexts = card.CardFaces
                .Where(f => !string.IsNullOrWhiteSpace(f.OracleText))
                .Select(f => f.OracleText);

            return string.Join("\n", faceTexts);
        }

        return string.Empty;
    }

    private static string GetTypeLine(Card card)
    {
        // First try the main type line
        if (!string.IsNullOrWhiteSpace(card.TypeLine))
        {
            return card.TypeLine;
        }

        // For multi-face cards, concatenate type lines
        if (card.CardFaces?.Count > 0)
        {
            var typeLines = card.CardFaces
                .Where(f => !string.IsNullOrWhiteSpace(f.TypeLine))
                .Select(f => f.TypeLine);

            return string.Join(" // ", typeLines);
        }

        return string.Empty;
    }

    private static bool MeetsTypeConstraints(string typeLine, CardPurpose purpose)
    {
        // Check required types - card must contain at least one
        if (!string.IsNullOrWhiteSpace(purpose.RequiredTypes))
        {
            var requiredTypes = purpose.RequiredTypes.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!requiredTypes.Any(t => typeLine.Contains(t, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
        }

        // Check excluded types - card must NOT contain any
        if (!string.IsNullOrWhiteSpace(purpose.ExcludedTypes))
        {
            var excludedTypes = purpose.ExcludedTypes.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (excludedTypes.Any(t => typeLine.Contains(t, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
        }

        return true;
    }

    private List<Regex> GetCompiledPatterns(CardPurpose purpose)
    {
        if (_compiledPatterns.TryGetValue(purpose.Id, out var cached))
        {
            return cached;
        }

        var patterns = purpose.Patterns!
            .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(p => new Regex(p, RegexOptions.IgnoreCase | RegexOptions.Compiled))
            .ToList();

        _compiledPatterns[purpose.Id] = patterns;
        return patterns;
    }

    private static decimal CalculateConfidence(Match match, string oracleText)
    {
        // Base confidence is 0.70
        var confidence = 0.70m;

        // Boost for longer matches (more specific)
        if (match.Length > 20)
            confidence += 0.10m;
        else if (match.Length > 10)
            confidence += 0.05m;

        // Boost if match is a significant portion of the text
        var matchRatio = (decimal)match.Length / oracleText.Length;
        if (matchRatio > 0.3m)
            confidence += 0.10m;
        else if (matchRatio > 0.1m)
            confidence += 0.05m;

        // Cap at 1.0
        return Math.Min(confidence, 1.0m);
    }
}
