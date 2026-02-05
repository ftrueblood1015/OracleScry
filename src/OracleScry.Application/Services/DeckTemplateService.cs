using Microsoft.EntityFrameworkCore;
using OracleScry.Application.DTOs.Decks;
using OracleScry.Application.DTOs.DeckTemplates;
using OracleScry.Application.Interfaces;
using OracleScry.Domain.Entities;
using OracleScry.Infrastructure.Persistence;

namespace OracleScry.Application.Services;

/// <summary>
/// Service for deck template operations.
/// Templates are publicly readable; creating a deck from a template requires authentication.
/// </summary>
public class DeckTemplateService(OracleScryDbContext context) : IDeckTemplateService
{
    private readonly OracleScryDbContext _context = context;

    public async Task<IEnumerable<DeckTemplateSummaryDto>> GetTemplatesAsync(
        string? format = null,
        string? search = null,
        CancellationToken ct = default)
    {
        var query = _context.DeckTemplates
            .AsNoTracking()
            .Where(dt => dt.IsActive)
            .Include(dt => dt.TemplateCards)
                .ThenInclude(tc => tc.Card)
            .AsQueryable();

        // Apply format filter
        if (!string.IsNullOrWhiteSpace(format))
        {
            query = query.Where(dt => dt.Format == format);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(dt =>
                dt.Name.ToLower().Contains(searchLower) ||
                (dt.Description != null && dt.Description.ToLower().Contains(searchLower)) ||
                (dt.SetName != null && dt.SetName.ToLower().Contains(searchLower)));
        }

        var templates = await query
            .OrderByDescending(dt => dt.ReleasedAt)
            .ThenBy(dt => dt.Name)
            .ToListAsync(ct);

        return templates.Select(dt => new DeckTemplateSummaryDto(
            dt.Id,
            dt.Name,
            dt.Description,
            dt.Format,
            dt.SetName,
            dt.TemplateCards.Where(tc => !tc.IsSideboard).Sum(tc => tc.Quantity),
            dt.TemplateCards.Where(tc => tc.IsSideboard).Sum(tc => tc.Quantity),
            dt.TemplateCards.FirstOrDefault(tc => !tc.IsSideboard && !tc.IsCommander)?.Card.ImageUris?.Normal
                ?? dt.TemplateCards.FirstOrDefault(tc => tc.IsCommander)?.Card.ImageUris?.Normal,
            dt.ReleasedAt
        ));
    }

    public async Task<DeckTemplateDto?> GetByIdAsync(Guid templateId, CancellationToken ct = default)
    {
        var template = await _context.DeckTemplates
            .AsNoTracking()
            .Where(dt => dt.Id == templateId && dt.IsActive)
            .Include(dt => dt.TemplateCards)
                .ThenInclude(tc => tc.Card)
            .FirstOrDefaultAsync(ct);

        if (template is null) return null;

        return MapToDto(template);
    }

    public async Task<DeckDto?> CreateDeckFromTemplateAsync(
        Guid userId,
        Guid templateId,
        CreateDeckFromTemplateRequest request,
        CancellationToken ct = default)
    {
        // Load template with all cards
        var template = await _context.DeckTemplates
            .AsNoTracking()
            .Where(dt => dt.Id == templateId && dt.IsActive)
            .Include(dt => dt.TemplateCards)
                .ThenInclude(tc => tc.Card)
                    .ThenInclude(c => c.CardPurposes)
                        .ThenInclude(cp => cp.CardPurpose)
            .FirstOrDefaultAsync(ct);

        if (template is null) return null;

        // Create new deck
        var deck = new Deck
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            Format = request.CopyFormat ? template.Format : null,
            IsPublic = false
        };

        _context.Decks.Add(deck);

        // Copy all template cards to deck cards
        var deckCards = template.TemplateCards.Select(tc => new DeckCard
        {
            DeckId = deck.Id,
            CardId = tc.CardId,
            Quantity = tc.Quantity,
            IsSideboard = tc.IsSideboard,
            AddedAt = DateTime.UtcNow
        }).ToList();

        _context.DeckCards.AddRange(deckCards);

        await _context.SaveChangesAsync(ct);

        // Build response DTO
        var cardDtos = template.TemplateCards.Select(tc => new DeckCardDto(
            tc.Card.Id,
            tc.Card.Name,
            tc.Card.ManaCost,
            tc.Card.Cmc,
            tc.Card.TypeLine,
            tc.Card.Rarity,
            tc.Card.ImageUris?.Normal,
            tc.Quantity,
            tc.IsSideboard,
            tc.Card.CardPurposes.Select(cp => cp.CardPurpose.Name).ToList()
        )).ToList();

        return new DeckDto(
            deck.Id,
            deck.Name,
            deck.Description,
            deck.Format,
            deck.IsPublic,
            cardDtos.Where(c => !c.IsSideboard).Sum(c => c.Quantity),
            cardDtos.Where(c => c.IsSideboard).Sum(c => c.Quantity),
            deck.ImportedOn,
            deck.LastUpdatedOn,
            cardDtos
        );
    }

    private static DeckTemplateDto MapToDto(DeckTemplate template)
    {
        var cards = template.TemplateCards
            .OrderByDescending(tc => tc.IsCommander) // Commanders first
            .ThenBy(tc => tc.IsSideboard) // Then mainboard
            .ThenBy(tc => tc.Card.Cmc) // Then by mana cost
            .ThenBy(tc => tc.Card.Name) // Then alphabetically
            .Select(tc => new DeckTemplateCardDto(
                tc.Card.Id,
                tc.Card.Name,
                tc.Card.ManaCost,
                tc.Card.Cmc,
                tc.Card.TypeLine,
                tc.Card.Rarity,
                tc.Card.ImageUris?.Normal,
                tc.Quantity,
                tc.IsSideboard,
                tc.IsCommander
            )).ToList();

        return new DeckTemplateDto(
            template.Id,
            template.Name,
            template.Description,
            template.Format,
            template.SetCode,
            template.SetName,
            template.ReleasedAt,
            cards.Where(c => !c.IsSideboard).Sum(c => c.Quantity),
            cards.Where(c => c.IsSideboard).Sum(c => c.Quantity),
            cards
        );
    }
}
