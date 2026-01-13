using Microsoft.EntityFrameworkCore;
using OracleScry.Application.DTOs.Decks;
using OracleScry.Application.Interfaces;
using OracleScry.Domain.Entities;
using OracleScry.Infrastructure.Persistence;

namespace OracleScry.Application.Services;

/// <summary>
/// Service for deck management with ownership validation.
/// </summary>
public class DeckService(OracleScryDbContext context) : IDeckService
{
    private readonly OracleScryDbContext _context = context;

    public async Task<IEnumerable<DeckSummaryDto>> GetUserDecksAsync(Guid userId, CancellationToken ct = default)
    {
        var decks = await _context.Decks
            .AsNoTracking()
            .Where(d => d.UserId == userId)
            .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
            .OrderByDescending(d => d.LastUpdatedOn)
            .ToListAsync(ct);

        return decks.Select(d => new DeckSummaryDto(
            d.Id,
            d.Name,
            d.Description,
            d.Format,
            d.DeckCards.Where(dc => !dc.IsSideboard).Sum(dc => dc.Quantity),
            d.DeckCards.Where(dc => dc.IsSideboard).Sum(dc => dc.Quantity),
            d.DeckCards.FirstOrDefault(dc => !dc.IsSideboard)?.Card.ImageUris?.Normal,
            d.LastUpdatedOn
        ));
    }

    public async Task<DeckDto?> GetByIdAsync(Guid deckId, Guid userId, CancellationToken ct = default)
    {
        var deck = await _context.Decks
            .AsNoTracking()
            .Where(d => d.Id == deckId && d.UserId == userId)
            .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
                    .ThenInclude(c => c.CardPurposes)
                        .ThenInclude(cp => cp.CardPurpose)
            .FirstOrDefaultAsync(ct);

        if (deck is null) return null;

        return MapToDto(deck);
    }

    public async Task<DeckDto> CreateAsync(Guid userId, CreateDeckRequest request, CancellationToken ct = default)
    {
        var deck = new Deck
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            Format = request.Format,
            IsPublic = false
        };

        _context.Decks.Add(deck);
        await _context.SaveChangesAsync(ct);

        return new DeckDto(
            deck.Id,
            deck.Name,
            deck.Description,
            deck.Format,
            deck.IsPublic,
            0,
            0,
            deck.ImportedOn,
            deck.LastUpdatedOn,
            []
        );
    }

    public async Task<DeckDto?> UpdateAsync(Guid deckId, Guid userId, UpdateDeckRequest request, CancellationToken ct = default)
    {
        var deck = await _context.Decks
            .Where(d => d.Id == deckId && d.UserId == userId)
            .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
                    .ThenInclude(c => c.CardPurposes)
                        .ThenInclude(cp => cp.CardPurpose)
            .FirstOrDefaultAsync(ct);

        if (deck is null) return null;

        if (request.Name is not null) deck.Name = request.Name;
        if (request.Description is not null) deck.Description = request.Description;
        if (request.Format is not null) deck.Format = request.Format;
        if (request.IsPublic.HasValue) deck.IsPublic = request.IsPublic.Value;

        await _context.SaveChangesAsync(ct);

        return MapToDto(deck);
    }

    public async Task<bool> DeleteAsync(Guid deckId, Guid userId, CancellationToken ct = default)
    {
        var deck = await _context.Decks
            .Where(d => d.Id == deckId && d.UserId == userId)
            .FirstOrDefaultAsync(ct);

        if (deck is null) return false;

        _context.Decks.Remove(deck);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<DeckCardDto?> AddCardAsync(Guid deckId, Guid userId, AddCardRequest request, CancellationToken ct = default)
    {
        // Verify deck ownership
        var deck = await _context.Decks
            .Where(d => d.Id == deckId && d.UserId == userId)
            .FirstOrDefaultAsync(ct);

        if (deck is null) return null;

        // Check if card exists
        var card = await _context.Cards
            .AsNoTracking()
            .Include(c => c.CardPurposes)
                .ThenInclude(cp => cp.CardPurpose)
            .FirstOrDefaultAsync(c => c.Id == request.CardId, ct);

        if (card is null) return null;

        // Check if card already in deck
        var existingCard = await _context.DeckCards
            .Where(dc => dc.DeckId == deckId && dc.CardId == request.CardId)
            .FirstOrDefaultAsync(ct);

        if (existingCard is not null)
        {
            // Update quantity instead
            existingCard.Quantity += request.Quantity;
            existingCard.IsSideboard = request.IsSideboard;
            await _context.SaveChangesAsync(ct);
        }
        else
        {
            // Add new card
            existingCard = new DeckCard
            {
                DeckId = deckId,
                CardId = request.CardId,
                Quantity = request.Quantity,
                IsSideboard = request.IsSideboard,
                AddedAt = DateTime.UtcNow
            };
            _context.DeckCards.Add(existingCard);
            await _context.SaveChangesAsync(ct);
        }

        return new DeckCardDto(
            card.Id,
            card.Name,
            card.ManaCost,
            card.Cmc,
            card.TypeLine,
            card.Rarity,
            card.ImageUris?.Normal,
            existingCard.Quantity,
            existingCard.IsSideboard,
            card.CardPurposes.Select(cp => cp.CardPurpose.Name).ToList()
        );
    }

    public async Task<DeckCardDto?> UpdateCardAsync(Guid deckId, Guid cardId, Guid userId, UpdateCardRequest request, CancellationToken ct = default)
    {
        // Verify deck ownership
        var deckOwned = await _context.Decks
            .AnyAsync(d => d.Id == deckId && d.UserId == userId, ct);

        if (!deckOwned) return null;

        var deckCard = await _context.DeckCards
            .Include(dc => dc.Card)
                .ThenInclude(c => c.CardPurposes)
                    .ThenInclude(cp => cp.CardPurpose)
            .Where(dc => dc.DeckId == deckId && dc.CardId == cardId)
            .FirstOrDefaultAsync(ct);

        if (deckCard is null) return null;

        if (request.Quantity.HasValue) deckCard.Quantity = request.Quantity.Value;
        if (request.IsSideboard.HasValue) deckCard.IsSideboard = request.IsSideboard.Value;

        await _context.SaveChangesAsync(ct);

        return new DeckCardDto(
            deckCard.Card.Id,
            deckCard.Card.Name,
            deckCard.Card.ManaCost,
            deckCard.Card.Cmc,
            deckCard.Card.TypeLine,
            deckCard.Card.Rarity,
            deckCard.Card.ImageUris?.Normal,
            deckCard.Quantity,
            deckCard.IsSideboard,
            deckCard.Card.CardPurposes.Select(cp => cp.CardPurpose.Name).ToList()
        );
    }

    public async Task<bool> RemoveCardAsync(Guid deckId, Guid cardId, Guid userId, CancellationToken ct = default)
    {
        // Verify deck ownership
        var deckOwned = await _context.Decks
            .AnyAsync(d => d.Id == deckId && d.UserId == userId, ct);

        if (!deckOwned) return false;

        var deckCard = await _context.DeckCards
            .Where(dc => dc.DeckId == deckId && dc.CardId == cardId)
            .FirstOrDefaultAsync(ct);

        if (deckCard is null) return false;

        _context.DeckCards.Remove(deckCard);
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<DeckStatsDto?> GetStatsAsync(Guid deckId, Guid userId, CancellationToken ct = default)
    {
        var deck = await _context.Decks
            .AsNoTracking()
            .Where(d => d.Id == deckId && d.UserId == userId)
            .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
                    .ThenInclude(c => c.CardPurposes)
                        .ThenInclude(cp => cp.CardPurpose)
            .FirstOrDefaultAsync(ct);

        if (deck is null) return null;

        var cards = deck.DeckCards.ToList();
        var mainboard = cards.Where(dc => !dc.IsSideboard).ToList();
        var sideboard = cards.Where(dc => dc.IsSideboard).ToList();

        // Mana curve (by CMC)
        var manaCurve = mainboard
            .GroupBy(dc => (int)Math.Min(dc.Card.Cmc, 7)) // Cap at 7+
            .ToDictionary(g => g.Key, g => g.Sum(dc => dc.Quantity));

        // Color distribution
        var colorDistribution = new Dictionary<string, int>();
        foreach (var dc in mainboard)
        {
            var colors = dc.Card.Colors ?? [];
            foreach (var color in colors)
            {
                if (!colorDistribution.ContainsKey(color))
                    colorDistribution[color] = 0;
                colorDistribution[color] += dc.Quantity;
            }
            if (colors.Count == 0)
            {
                if (!colorDistribution.ContainsKey("C"))
                    colorDistribution["C"] = 0;
                colorDistribution["C"] += dc.Quantity;
            }
        }

        // Type distribution (extract primary type)
        var typeDistribution = mainboard
            .GroupBy(dc => GetPrimaryType(dc.Card.TypeLine))
            .ToDictionary(g => g.Key, g => g.Sum(dc => dc.Quantity));

        // Purpose breakdown
        var purposeBreakdown = new Dictionary<string, int>();
        foreach (var dc in mainboard)
        {
            foreach (var cp in dc.Card.CardPurposes)
            {
                var name = cp.CardPurpose.Name;
                if (!purposeBreakdown.ContainsKey(name))
                    purposeBreakdown[name] = 0;
                purposeBreakdown[name] += dc.Quantity;
            }
        }

        // Rarity distribution
        var rarityDistribution = mainboard
            .GroupBy(dc => dc.Card.Rarity ?? "unknown")
            .ToDictionary(g => g.Key, g => g.Sum(dc => dc.Quantity));

        // Average CMC (excluding lands)
        var nonLandCards = mainboard.Where(dc => !dc.Card.TypeLine.Contains("Land")).ToList();
        var avgCmc = nonLandCards.Count > 0
            ? nonLandCards.Sum(dc => dc.Card.Cmc * dc.Quantity) / nonLandCards.Sum(dc => dc.Quantity)
            : 0;

        // Estimated price
        decimal? totalPrice = null;
        try
        {
            totalPrice = cards.Sum(dc =>
            {
                if (decimal.TryParse(dc.Card.Prices?.Usd, out var price))
                    return price * dc.Quantity;
                return 0;
            });
        }
        catch { /* Price calculation failed */ }

        return new DeckStatsDto(
            cards.Sum(dc => dc.Quantity),
            mainboard.Sum(dc => dc.Quantity),
            sideboard.Sum(dc => dc.Quantity),
            cards.Count,
            manaCurve,
            colorDistribution,
            typeDistribution,
            purposeBreakdown,
            rarityDistribution,
            avgCmc,
            totalPrice
        );
    }

    private static string GetPrimaryType(string typeLine)
    {
        var types = new[] { "Creature", "Instant", "Sorcery", "Enchantment", "Artifact", "Planeswalker", "Land", "Battle" };
        foreach (var type in types)
        {
            if (typeLine.Contains(type, StringComparison.OrdinalIgnoreCase))
                return type;
        }
        return "Other";
    }

    private static DeckDto MapToDto(Deck deck)
    {
        var cards = deck.DeckCards.Select(dc => new DeckCardDto(
            dc.Card.Id,
            dc.Card.Name,
            dc.Card.ManaCost,
            dc.Card.Cmc,
            dc.Card.TypeLine,
            dc.Card.Rarity,
            dc.Card.ImageUris?.Normal,
            dc.Quantity,
            dc.IsSideboard,
            dc.Card.CardPurposes.Select(cp => cp.CardPurpose.Name).ToList()
        )).ToList();

        return new DeckDto(
            deck.Id,
            deck.Name,
            deck.Description,
            deck.Format,
            deck.IsPublic,
            cards.Where(c => !c.IsSideboard).Sum(c => c.Quantity),
            cards.Where(c => c.IsSideboard).Sum(c => c.Quantity),
            deck.ImportedOn,
            deck.LastUpdatedOn,
            cards
        );
    }
}
