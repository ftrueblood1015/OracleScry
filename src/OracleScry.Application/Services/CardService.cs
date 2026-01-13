using Microsoft.EntityFrameworkCore;
using OracleScry.Application.DTOs.Cards;
using OracleScry.Application.DTOs.Common;
using OracleScry.Application.Interfaces;
using OracleScry.Domain.Entities;
using OracleScry.Infrastructure.Persistence;

namespace OracleScry.Application.Services;

/// <summary>
/// Card service implementation with optimized queries.
/// Uses direct DbContext for complex queries per EF Core best practices.
/// </summary>
public class CardService(OracleScryDbContext context) : ICardService
{
    private readonly OracleScryDbContext _context = context;

    public async Task<CardDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var card = await _context.Cards
            .AsNoTracking()
            .Include(c => c.CardFaces.OrderBy(cf => cf.FaceIndex))
            .Include(c => c.AllParts)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        return card is null ? null : MapToDto(card);
    }

    public async Task<CardDto?> GetByScryfallIdAsync(Guid scryfallId, CancellationToken ct = default)
    {
        var card = await _context.Cards
            .AsNoTracking()
            .Include(c => c.CardFaces.OrderBy(cf => cf.FaceIndex))
            .Include(c => c.AllParts)
            .FirstOrDefaultAsync(c => c.ScryfallId == scryfallId, ct);

        return card is null ? null : MapToDto(card);
    }

    public async Task<PagedResult<CardSummaryDto>> SearchAsync(CardFilterDto filter, CancellationToken ct = default)
    {
        var page = filter.GetValidatedPage();
        var pageSize = filter.GetValidatedPageSize();

        // Build base query - use raw SQL if color filter is specified
        // (EF Core value converters don't work with EF.Property for LIKE queries)
        IQueryable<Card> query;

        if (filter.Colors?.Count > 0)
        {
            // Build raw SQL for color filtering
            // Colors are stored as comma-separated strings (e.g., "W,U,B")
            // Validate color inputs to prevent SQL injection (only allow valid MTG color codes)
            var validColors = new HashSet<string> { "W", "U", "B", "R", "G", "C" };
            var sanitizedColors = filter.Colors
                .Select(c => c.ToUpper())
                .Where(c => validColors.Contains(c))
                .ToList();

            if (sanitizedColors.Count > 0)
            {
                var colorConditions = sanitizedColors.Select(c =>
                    // Match: exact value, starts with "C,", ends with ",C", or contains ",C,"
                    $"(Colors = '{c}' OR Colors LIKE '{c},%' OR Colors LIKE '%,{c}' OR Colors LIKE '%,{c},%')"
                );
                var colorWhere = string.Join(" AND ", colorConditions);

                query = _context.Cards
                    .FromSqlRaw($"SELECT * FROM Cards WHERE {colorWhere}")
                    .AsNoTracking();
            }
            else
            {
                query = _context.Cards.AsNoTracking();
            }
        }
        else
        {
            query = _context.Cards.AsNoTracking();
        }

        // Apply other filters
        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            var searchTerm = filter.Query.ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                (c.OracleText != null && c.OracleText.ToLower().Contains(searchTerm)) ||
                c.TypeLine.ToLower().Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(filter.SetCode))
        {
            query = query.Where(c => c.SetCode == filter.SetCode.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(filter.Rarity))
        {
            query = query.Where(c => c.Rarity == filter.Rarity.ToLower());
        }

        if (filter.MinCmc.HasValue)
        {
            query = query.Where(c => c.Cmc >= filter.MinCmc.Value);
        }

        if (filter.MaxCmc.HasValue)
        {
            query = query.Where(c => c.Cmc <= filter.MaxCmc.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.TypeLine))
        {
            var typeTerm = filter.TypeLine.ToLower();
            query = query.Where(c => c.TypeLine.ToLower().Contains(typeTerm));
        }

        // Apply format legality filter
        if (!string.IsNullOrWhiteSpace(filter.Format))
        {
            query = ApplyFormatFilter(query, filter.Format);
        }

        // Apply purpose filter
        if (filter.PurposeIds?.Count > 0)
        {
            query = query.Where(c => c.CardPurposes.Any(cp => filter.PurposeIds.Contains(cp.CardPurposeId)));
        }

        // Get total count
        var totalCount = await query.CountAsync(ct);

        // Apply sorting
        query = ApplySorting(query, filter.SortBy, filter.SortDescending);

        // Apply pagination
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CardSummaryDto(
                c.Id,
                c.Name,
                c.ManaCost,
                c.TypeLine,
                c.Rarity,
                c.SetCode,
                c.SetName,
                c.ImageUris != null ? c.ImageUris.Normal : null,
                c.Prices.Usd))
            .ToListAsync(ct);

        return new PagedResult<CardSummaryDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<IEnumerable<CardSummaryDto>> GetBySetAsync(string setCode, CancellationToken ct = default)
    {
        return await _context.Cards
            .AsNoTracking()
            .Where(c => c.SetCode == setCode.ToLower())
            .OrderBy(c => c.CollectorNumber)
            .Select(c => new CardSummaryDto(
                c.Id,
                c.Name,
                c.ManaCost,
                c.TypeLine,
                c.Rarity,
                c.SetCode,
                c.SetName,
                c.ImageUris != null ? c.ImageUris.Normal : null,
                c.Prices.Usd))
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<string>> GetSetsAsync(CancellationToken ct = default)
    {
        return await _context.Cards
            .AsNoTracking()
            .Select(c => c.SetCode)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<CardSummaryDto>> GetRandomCardsAsync(int count = 10, CancellationToken ct = default)
    {
        return await _context.Cards
            .AsNoTracking()
            .OrderBy(c => Guid.NewGuid())
            .Take(count)
            .Select(c => new CardSummaryDto(
                c.Id,
                c.Name,
                c.ManaCost,
                c.TypeLine,
                c.Rarity,
                c.SetCode,
                c.SetName,
                c.ImageUris != null ? c.ImageUris.Normal : null,
                c.Prices.Usd))
            .ToListAsync(ct);
    }

    private static IQueryable<Card> ApplyFormatFilter(IQueryable<Card> query, string format)
    {
        return format.ToLower() switch
        {
            "standard" => query.Where(c => c.Legalities.Standard == "legal"),
            "pioneer" => query.Where(c => c.Legalities.Pioneer == "legal"),
            "modern" => query.Where(c => c.Legalities.Modern == "legal"),
            "legacy" => query.Where(c => c.Legalities.Legacy == "legal"),
            "vintage" => query.Where(c => c.Legalities.Vintage == "legal" || c.Legalities.Vintage == "restricted"),
            "commander" => query.Where(c => c.Legalities.Commander == "legal"),
            "pauper" => query.Where(c => c.Legalities.Pauper == "legal"),
            "historic" => query.Where(c => c.Legalities.Historic == "legal"),
            "explorer" => query.Where(c => c.Legalities.Explorer == "legal"),
            "brawl" => query.Where(c => c.Legalities.Brawl == "legal"),
            _ => query
        };
    }

    private static IQueryable<Card> ApplySorting(IQueryable<Card> query, string sortBy, bool descending)
    {
        return sortBy.ToLower() switch
        {
            "name" => descending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            "cmc" => descending ? query.OrderByDescending(c => c.Cmc) : query.OrderBy(c => c.Cmc),
            "rarity" => descending ? query.OrderByDescending(c => c.Rarity) : query.OrderBy(c => c.Rarity),
            "set" => descending ? query.OrderByDescending(c => c.SetCode) : query.OrderBy(c => c.SetCode),
            "released" => descending ? query.OrderByDescending(c => c.ReleasedAt) : query.OrderBy(c => c.ReleasedAt),
            "price" => descending ? query.OrderByDescending(c => c.Prices.Usd) : query.OrderBy(c => c.Prices.Usd),
            _ => query.OrderBy(c => c.Name)
        };
    }

    private static CardDto MapToDto(Card card)
    {
        return new CardDto(
            card.Id,
            card.ScryfallId,
            card.OracleId,
            card.Name,
            card.Lang,
            card.Layout,
            card.ManaCost,
            card.Cmc,
            card.TypeLine,
            card.OracleText,
            card.Power,
            card.Toughness,
            card.Loyalty,
            card.Defense,
            card.Colors,
            card.ColorIdentity,
            card.Keywords,
            card.SetCode,
            card.SetName,
            card.SetType,
            card.CollectorNumber,
            card.Rarity,
            card.ReleasedAt,
            card.Reprint,
            card.Digital,
            card.Artist,
            card.FlavorText,
            card.BorderColor,
            card.Frame,
            card.FullArt,
            card.Textless,
            card.Promo,
            card.Reserved,
            card.ImageUris is null ? null : new CardImageUrisDto(
                card.ImageUris.Small,
                card.ImageUris.Normal,
                card.ImageUris.Large,
                card.ImageUris.Png,
                card.ImageUris.ArtCrop,
                card.ImageUris.BorderCrop),
            new CardPricesDto(
                card.Prices.Usd,
                card.Prices.UsdFoil,
                card.Prices.UsdEtched,
                card.Prices.Eur,
                card.Prices.EurFoil,
                card.Prices.EurEtched,
                card.Prices.Tix),
            new CardLegalitiesDto(
                card.Legalities.Standard,
                card.Legalities.Future,
                card.Legalities.Historic,
                card.Legalities.Timeless,
                card.Legalities.Gladiator,
                card.Legalities.Pioneer,
                card.Legalities.Explorer,
                card.Legalities.Modern,
                card.Legalities.Legacy,
                card.Legalities.Pauper,
                card.Legalities.Vintage,
                card.Legalities.Penny,
                card.Legalities.Commander,
                card.Legalities.Oathbreaker,
                card.Legalities.StandardBrawl,
                card.Legalities.Brawl,
                card.Legalities.Alchemy,
                card.Legalities.PauperCommander,
                card.Legalities.Duel,
                card.Legalities.Oldschool,
                card.Legalities.Premodern,
                card.Legalities.Predh),
            card.CardFaces.Count > 0 ? card.CardFaces.Select(cf => new CardFaceDto(
                cf.Id,
                cf.Name,
                cf.ManaCost,
                cf.TypeLine,
                cf.OracleText,
                cf.Colors,
                cf.Power,
                cf.Toughness,
                cf.Loyalty,
                cf.Defense,
                cf.FlavorText,
                cf.ImageUris is null ? null : new CardImageUrisDto(
                    cf.ImageUris.Small,
                    cf.ImageUris.Normal,
                    cf.ImageUris.Large,
                    cf.ImageUris.Png,
                    cf.ImageUris.ArtCrop,
                    cf.ImageUris.BorderCrop),
                cf.Artist,
                cf.FaceIndex)).ToList() : null,
            card.AllParts.Count > 0 ? card.AllParts.Select(rp => new RelatedCardDto(
                rp.Id,
                rp.RelatedCardScryfallId,
                rp.Component,
                rp.Name,
                rp.TypeLine,
                rp.Uri)).ToList() : null,
            card.ScryfallUri,
            card.RulingsUri,
            card.ImportedOn,
            card.LastUpdatedOn);
    }
}
