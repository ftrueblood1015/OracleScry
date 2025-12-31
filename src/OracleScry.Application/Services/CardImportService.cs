using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OracleScry.Application.DTOs.Common;
using OracleScry.Application.DTOs.Import;
using OracleScry.Application.Interfaces;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;
using OracleScry.Domain.Interfaces;
using OracleScry.Infrastructure.Persistence;
using ScryfallBulkDataInfo = OracleScry.Domain.Interfaces.ScryfallBulkDataInfo;

namespace OracleScry.Application.Services;

/// <summary>
/// Service for importing cards from Scryfall bulk data.
/// Uses optimized batch processing to minimize database hits.
/// </summary>
public class CardImportService : ICardImportService
{
    private readonly OracleScryDbContext _context;
    private readonly ICardImportRepository _importRepository;
    private readonly IScryfallApiClient _scryfallClient;
    private readonly ILogger<CardImportService> _logger;
    private const int BatchSize = 1000;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public CardImportService(
        OracleScryDbContext context,
        ICardImportRepository importRepository,
        IScryfallApiClient scryfallClient,
        ILogger<CardImportService> logger)
    {
        _context = context;
        _importRepository = importRepository;
        _scryfallClient = scryfallClient;
        _logger = logger;
    }

    public async Task<CardImportDto> ExecuteImportAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting Scryfall card import");

        // Check for running import
        if (await _importRepository.HasRunningImportAsync(ct))
        {
            throw new InvalidOperationException("An import is already running");
        }

        // Create import record
        var import = new CardImport
        {
            Id = Guid.NewGuid(),
            StartedAt = DateTime.UtcNow,
            Status = CardImportStatus.Pending
        };

        await _importRepository.AddAsync(import, ct);
        await _context.SaveChangesAsync(ct);

        try
        {
            // Get bulk data info
            import.Status = CardImportStatus.Downloading;
            await _context.SaveChangesAsync(ct);

            var bulkDataInfo = await _scryfallClient.GetBulkDataInfoAsync(ct);
            import.BulkDataId = bulkDataInfo.Id;
            import.DownloadUri = bulkDataInfo.DownloadUri;
            import.ScryfallUpdatedAt = bulkDataInfo.UpdatedAt;
            import.FileSizeBytes = bulkDataInfo.Size;
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Downloading bulk data: {Size} bytes", bulkDataInfo.Size);

            // Download and process
            import.Status = CardImportStatus.Processing;
            await _context.SaveChangesAsync(ct);

            await using var stream = await _scryfallClient.DownloadBulkDataAsync(bulkDataInfo.DownloadUri, ct);
            await ProcessBulkDataAsync(import, stream, ct);

            // Complete
            import.Status = CardImportStatus.Completed;
            import.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Import completed: {Added} added, {Updated} updated, {Skipped} skipped, {Failed} failed",
                import.CardsAdded, import.CardsUpdated, import.CardsSkipped, import.CardsFailed);

            return MapToDto(import);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Import failed");
            import.Status = CardImportStatus.Failed;
            import.ErrorMessage = ex.Message;
            import.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);
            throw;
        }
    }

    private async Task ProcessBulkDataAsync(CardImport import, Stream stream, CancellationToken ct)
    {
        // Load existing oracle IDs for O(1) lookup
        _logger.LogInformation("Loading existing card oracle IDs");
        var existingCards = await _context.Cards
            .Where(c => c.OracleId != null)
            .Select(c => new { c.OracleId, c.Id, c.LastUpdatedOn })
            .ToDictionaryAsync(c => c.OracleId!.Value, ct);

        _logger.LogInformation("Found {Count} existing cards", existingCards.Count);

        var cardsToInsert = new List<Card>();
        var cardsToUpdate = new List<(Guid Id, ScryfallCard Data)>();

        // Stream parse JSON array
        var cards = JsonSerializer.DeserializeAsyncEnumerable<ScryfallCard>(stream, JsonOptions, ct);
        int totalProcessed = 0;

        await foreach (var scryfallCard in cards)
        {
            if (scryfallCard is null) continue;

            import.TotalCardsInFile++;
            totalProcessed++;

            try
            {
                if (scryfallCard.OracleId.HasValue && existingCards.TryGetValue(scryfallCard.OracleId.Value, out var existing))
                {
                    // Card exists - queue for update
                    cardsToUpdate.Add((existing.Id, scryfallCard));
                }
                else
                {
                    // New card - queue for insert
                    cardsToInsert.Add(MapToEntity(scryfallCard));
                }

                // Process batches
                if (cardsToInsert.Count >= BatchSize)
                {
                    await InsertBatchAsync(import, cardsToInsert, ct);
                    cardsToInsert.Clear();
                }

                if (cardsToUpdate.Count >= BatchSize)
                {
                    await UpdateBatchAsync(import, cardsToUpdate, ct);
                    cardsToUpdate.Clear();
                }

                // Update progress periodically
                if (totalProcessed % 5000 == 0)
                {
                    import.CardsProcessed = totalProcessed;
                    await _context.SaveChangesAsync(ct);
                    _logger.LogInformation("Processed {Count} cards", totalProcessed);
                }
            }
            catch (Exception ex)
            {
                import.CardsFailed++;
                import.Errors.Add(new CardImportError
                {
                    Id = Guid.NewGuid(),
                    CardImportId = import.Id,
                    OracleId = scryfallCard.OracleId,
                    CardName = scryfallCard.Name,
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        // Process remaining cards
        if (cardsToInsert.Count > 0)
        {
            await InsertBatchAsync(import, cardsToInsert, ct);
        }

        if (cardsToUpdate.Count > 0)
        {
            await UpdateBatchAsync(import, cardsToUpdate, ct);
        }

        import.CardsProcessed = totalProcessed;
    }

    private async Task InsertBatchAsync(CardImport import, List<Card> cards, CancellationToken ct)
    {
        var addedCount = cards.Count;

        await _context.Cards.AddRangeAsync(cards, ct);
        await _context.SaveChangesAsync(ct);

        // Clear change tracker to prevent memory bloat and stale entity issues
        // But re-attach the import entity so it continues to be tracked
        _context.ChangeTracker.Clear();
        _context.Attach(import);

        // Modify AFTER re-attach so EF detects the change
        import.CardsAdded += addedCount;
    }

    private async Task UpdateBatchAsync(CardImport import, List<(Guid Id, ScryfallCard Data)> cards, CancellationToken ct)
    {
        // Deduplicate by Card.Id - keep only first occurrence (same oracle_id can appear multiple times)
        var deduplicatedCards = cards
            .GroupBy(c => c.Id)
            .Select(g => g.First())
            .ToList();

        var cardIds = deduplicatedCards.Select(c => c.Id).ToList();

        // Track counts locally to apply after re-attach
        var updatedCount = 0;
        var skippedCount = 0;

        // First, delete existing CardFaces and RelatedCards for these cards
        // This prevents concurrency issues with Clear() on tracked collections
        await _context.Set<CardFace>()
            .Where(cf => cardIds.Contains(cf.CardId))
            .ExecuteDeleteAsync(ct);

        await _context.Set<RelatedCard>()
            .Where(rc => cardIds.Contains(rc.CardId))
            .ExecuteDeleteAsync(ct);

        // Now load cards without their collections (they've been deleted)
        var existingCards = await _context.Cards
            .Where(c => cardIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, ct);

        foreach (var (id, data) in deduplicatedCards)
        {
            if (!existingCards.TryGetValue(id, out var card))
            {
                skippedCount++;
                continue;
            }

            UpdateEntityWithoutCollections(card, data);

            // Add new CardFaces
            if (data.CardFaces is not null)
            {
                var faceIndex = 0;
                foreach (var cf in data.CardFaces)
                {
                    _context.Set<CardFace>().Add(MapToCardFace(cf, card.Id, faceIndex++));
                }
            }

            // Add new RelatedCards
            if (data.AllParts is not null)
            {
                foreach (var ap in data.AllParts)
                {
                    _context.Set<RelatedCard>().Add(MapToRelatedCard(ap, card.Id));
                }
            }

            updatedCount++;
        }

        // Account for skipped duplicates
        skippedCount += cards.Count - deduplicatedCards.Count;

        await _context.SaveChangesAsync(ct);

        // Clear change tracker to prevent memory bloat and stale entity issues
        // But re-attach the import entity so it continues to be tracked
        _context.ChangeTracker.Clear();
        _context.Attach(import);

        // Modify AFTER re-attach so EF detects the changes
        import.CardsUpdated += updatedCount;
        import.CardsSkipped += skippedCount;
    }

    private static CardFace MapToCardFace(ScryfallCardFace cf, Guid cardId, int faceIndex)
    {
        return new CardFace
        {
            Id = Guid.NewGuid(),
            CardId = cardId,
            Object = cf.Object ?? "card_face",
            Name = cf.Name ?? string.Empty,
            ManaCost = cf.ManaCost ?? string.Empty,
            TypeLine = cf.TypeLine,
            OracleText = cf.OracleText,
            Colors = cf.Colors,
            ColorIndicator = cf.ColorIndicator,
            Power = cf.Power,
            Toughness = cf.Toughness,
            Loyalty = cf.Loyalty,
            Defense = cf.Defense,
            FlavorText = cf.FlavorText,
            IllustrationId = cf.IllustrationId,
            ImageUris = cf.ImageUris is null ? null : new Domain.ValueObjects.CardImageUris
            {
                Small = cf.ImageUris.Small,
                Normal = cf.ImageUris.Normal,
                Large = cf.ImageUris.Large,
                Png = cf.ImageUris.Png,
                ArtCrop = cf.ImageUris.ArtCrop,
                BorderCrop = cf.ImageUris.BorderCrop
            },
            Cmc = cf.Cmc,
            OracleId = cf.OracleId,
            Layout = cf.Layout,
            PrintedName = cf.PrintedName,
            PrintedText = cf.PrintedText,
            PrintedTypeLine = cf.PrintedTypeLine,
            Watermark = cf.Watermark,
            Artist = cf.Artist,
            ArtistId = cf.ArtistId,
            FaceIndex = faceIndex
        };
    }

    private static RelatedCard MapToRelatedCard(ScryfallRelatedCard ap, Guid cardId)
    {
        return new RelatedCard
        {
            Id = Guid.NewGuid(),
            CardId = cardId,
            Object = ap.Object ?? "related_card",
            RelatedCardScryfallId = ap.Id,
            Component = ap.Component ?? string.Empty,
            Name = ap.Name ?? string.Empty,
            TypeLine = ap.TypeLine ?? string.Empty,
            Uri = ap.Uri ?? string.Empty
        };
    }

    private static void UpdateEntityWithoutCollections(Card card, ScryfallCard sc)
    {
        card.Name = sc.Name ?? card.Name;
        card.OracleText = sc.OracleText;
        card.ManaCost = sc.ManaCost;
        card.Cmc = sc.Cmc;
        card.TypeLine = sc.TypeLine ?? card.TypeLine;
        card.Power = sc.Power;
        card.Toughness = sc.Toughness;
        card.Loyalty = sc.Loyalty;
        card.Defense = sc.Defense;
        card.Colors = sc.Colors ?? [];
        card.ColorIdentity = sc.ColorIdentity ?? [];
        card.Keywords = sc.Keywords ?? [];
        card.Reserved = sc.Reserved;
        card.EdhrecRank = sc.EdhrecRank;
        card.PennyRank = sc.PennyRank;

        // Update prices
        if (sc.Prices is not null)
        {
            card.Prices.Usd = sc.Prices.Usd;
            card.Prices.UsdFoil = sc.Prices.UsdFoil;
            card.Prices.UsdEtched = sc.Prices.UsdEtched;
            card.Prices.Eur = sc.Prices.Eur;
            card.Prices.EurFoil = sc.Prices.EurFoil;
            card.Prices.EurEtched = sc.Prices.EurEtched;
            card.Prices.Tix = sc.Prices.Tix;
        }

        // Update legalities
        if (sc.Legalities is not null)
        {
            card.Legalities.Standard = sc.Legalities.Standard;
            card.Legalities.Future = sc.Legalities.Future;
            card.Legalities.Historic = sc.Legalities.Historic;
            card.Legalities.Pioneer = sc.Legalities.Pioneer;
            card.Legalities.Modern = sc.Legalities.Modern;
            card.Legalities.Legacy = sc.Legalities.Legacy;
            card.Legalities.Vintage = sc.Legalities.Vintage;
            card.Legalities.Commander = sc.Legalities.Commander;
            card.Legalities.Pauper = sc.Legalities.Pauper;
        }
    }

    public async Task<PagedResult<CardImportSummaryDto>> GetHistoryAsync(int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var imports = await _importRepository.GetHistoryAsync(page, pageSize, ct);
        var totalCount = await _importRepository.GetCountAsync(ct);

        return new PagedResult<CardImportSummaryDto>
        {
            Items = imports.Select(MapToSummaryDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<CardImportDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var import = await _importRepository.GetByIdWithErrorsAsync(id, ct);
        return import is null ? null : MapToDto(import);
    }

    public async Task<CardImportSummaryDto?> GetLatestAsync(CancellationToken ct = default)
    {
        var import = await _importRepository.GetLatestAsync(ct);
        return import is null ? null : MapToSummaryDto(import);
    }

    public async Task<ImportStatsDto> GetStatsAsync(CancellationToken ct = default)
    {
        var (totalImports, successful, failed, totalAdded, totalUpdated) =
            await _importRepository.GetAggregatedStatsAsync(ct);

        var latestImport = await _importRepository.GetLatestAsync(ct);

        // Calculate average duration from completed imports
        var avgDuration = await _context.Set<CardImport>()
            .Where(i => i.Status == CardImportStatus.Completed && i.CompletedAt.HasValue)
            .Select(i => EF.Functions.DateDiffSecond(i.StartedAt, i.CompletedAt!.Value))
            .DefaultIfEmpty()
            .AverageAsync(ct);

        return new ImportStatsDto(
            totalImports,
            successful,
            failed,
            totalAdded,
            totalUpdated,
            latestImport?.StartedAt,
            avgDuration > 0 ? avgDuration : null
        );
    }

    public async Task<bool> IsImportRunningAsync(CancellationToken ct = default)
        => await _importRepository.HasRunningImportAsync(ct);

    private static CardImportDto MapToDto(CardImport import)
    {
        var duration = import.CompletedAt.HasValue
            ? (import.CompletedAt.Value - import.StartedAt).TotalSeconds
            : (double?)null;

        return new CardImportDto(
            import.Id,
            import.StartedAt,
            import.CompletedAt,
            import.Status.ToString(),
            import.TotalCardsInFile,
            import.CardsProcessed,
            import.CardsAdded,
            import.CardsUpdated,
            import.CardsSkipped,
            import.CardsFailed,
            import.BulkDataId,
            import.DownloadUri,
            import.ScryfallUpdatedAt,
            import.FileSizeBytes,
            import.ErrorMessage,
            duration,
            import.Errors.Count > 0
                ? import.Errors.Select(e => new CardImportErrorDto(
                    e.Id, e.OracleId, e.CardName, e.ErrorMessage)).ToList()
                : null
        );
    }

    private static CardImportSummaryDto MapToSummaryDto(CardImport import)
    {
        var duration = import.CompletedAt.HasValue
            ? (import.CompletedAt.Value - import.StartedAt).TotalSeconds
            : (double?)null;

        return new CardImportSummaryDto(
            import.Id,
            import.StartedAt,
            import.CompletedAt,
            import.Status.ToString(),
            import.CardsAdded,
            import.CardsUpdated,
            import.CardsFailed,
            duration
        );
    }

    private static Card MapToEntity(ScryfallCard sc)
    {
        return new Card
        {
            Id = Guid.NewGuid(),
            ScryfallId = sc.Id,
            OracleId = sc.OracleId,
            Name = sc.Name ?? string.Empty,
            Lang = sc.Lang ?? "en",
            Layout = sc.Layout ?? string.Empty,
            ManaCost = sc.ManaCost,
            Cmc = sc.Cmc,
            TypeLine = sc.TypeLine ?? string.Empty,
            OracleText = sc.OracleText,
            Power = sc.Power,
            Toughness = sc.Toughness,
            Loyalty = sc.Loyalty,
            Defense = sc.Defense,
            Colors = sc.Colors ?? [],
            ColorIdentity = sc.ColorIdentity ?? [],
            ColorIndicator = sc.ColorIndicator,
            ProducedMana = sc.ProducedMana,
            Keywords = sc.Keywords ?? [],
            Reserved = sc.Reserved,
            EdhrecRank = sc.EdhrecRank,
            PennyRank = sc.PennyRank,
            Artist = sc.Artist,
            ArtistIds = sc.ArtistIds ?? [],
            Booster = sc.Booster,
            BorderColor = sc.BorderColor ?? string.Empty,
            CardBackId = sc.CardBackId,
            CollectorNumber = sc.CollectorNumber ?? string.Empty,
            ContentWarning = sc.ContentWarning,
            Digital = sc.Digital,
            Finishes = sc.Finishes ?? [],
            FlavorName = sc.FlavorName,
            FlavorText = sc.FlavorText,
            FrameEffects = sc.FrameEffects,
            Frame = sc.Frame ?? string.Empty,
            FullArt = sc.FullArt,
            Games = sc.Games ?? [],
            HighresImage = sc.HighresImage,
            IllustrationId = sc.IllustrationId,
            ImageStatus = sc.ImageStatus ?? string.Empty,
            Oversized = sc.Oversized,
            PrintedName = sc.PrintedName,
            PrintedText = sc.PrintedText,
            PrintedTypeLine = sc.PrintedTypeLine,
            Promo = sc.Promo,
            PromoTypes = sc.PromoTypes,
            Rarity = sc.Rarity ?? string.Empty,
            ReleasedAt = sc.ReleasedAt,
            Reprint = sc.Reprint,
            SetCode = sc.Set ?? string.Empty,
            SetId = sc.SetId,
            SetName = sc.SetName ?? string.Empty,
            SetType = sc.SetType ?? string.Empty,
            SetUri = sc.SetUri ?? string.Empty,
            SetSearchUri = sc.SetSearchUri ?? string.Empty,
            ScryfallSetUri = sc.ScryfallSetUri ?? string.Empty,
            StorySpotlight = sc.StorySpotlight,
            Textless = sc.Textless,
            Variation = sc.Variation,
            VariationOf = sc.VariationOf,
            SecurityStamp = sc.SecurityStamp,
            Watermark = sc.Watermark,
            Uri = sc.Uri ?? string.Empty,
            ScryfallUri = sc.ScryfallUri ?? string.Empty,
            PrintsSearchUri = sc.PrintsSearchUri ?? string.Empty,
            RulingsUri = sc.RulingsUri ?? string.Empty,
            ArenaId = sc.ArenaId,
            MtgoId = sc.MtgoId,
            MtgoFoilId = sc.MtgoFoilId,
            MultiverseIds = sc.MultiverseIds ?? [],
            TcgplayerId = sc.TcgplayerId,
            TcgplayerEtchedId = sc.TcgplayerEtchedId,
            CardmarketId = sc.CardmarketId,
            Object = sc.Object ?? "card",
            ImageUris = sc.ImageUris is null ? null : new Domain.ValueObjects.CardImageUris
            {
                Small = sc.ImageUris.Small,
                Normal = sc.ImageUris.Normal,
                Large = sc.ImageUris.Large,
                Png = sc.ImageUris.Png,
                ArtCrop = sc.ImageUris.ArtCrop,
                BorderCrop = sc.ImageUris.BorderCrop
            },
            Prices = new Domain.ValueObjects.CardPrices
            {
                Usd = sc.Prices?.Usd,
                UsdFoil = sc.Prices?.UsdFoil,
                UsdEtched = sc.Prices?.UsdEtched,
                Eur = sc.Prices?.Eur,
                EurFoil = sc.Prices?.EurFoil,
                EurEtched = sc.Prices?.EurEtched,
                Tix = sc.Prices?.Tix
            },
            Legalities = new Domain.ValueObjects.CardLegalities
            {
                Standard = sc.Legalities?.Standard,
                Future = sc.Legalities?.Future,
                Historic = sc.Legalities?.Historic,
                Timeless = sc.Legalities?.Timeless,
                Gladiator = sc.Legalities?.Gladiator,
                Pioneer = sc.Legalities?.Pioneer,
                Explorer = sc.Legalities?.Explorer,
                Modern = sc.Legalities?.Modern,
                Legacy = sc.Legalities?.Legacy,
                Pauper = sc.Legalities?.Pauper,
                Vintage = sc.Legalities?.Vintage,
                Penny = sc.Legalities?.Penny,
                Commander = sc.Legalities?.Commander,
                Oathbreaker = sc.Legalities?.Oathbreaker,
                StandardBrawl = sc.Legalities?.StandardBrawl,
                Brawl = sc.Legalities?.Brawl,
                Alchemy = sc.Legalities?.Alchemy,
                PauperCommander = sc.Legalities?.PauperCommander,
                Duel = sc.Legalities?.Duel,
                Oldschool = sc.Legalities?.Oldschool,
                Premodern = sc.Legalities?.Premodern,
                Predh = sc.Legalities?.Predh
            },
            CardFaces = sc.CardFaces?.Select((cf, index) => new CardFace
            {
                Id = Guid.NewGuid(),
                Object = cf.Object ?? "card_face",
                Name = cf.Name ?? string.Empty,
                ManaCost = cf.ManaCost ?? string.Empty,
                TypeLine = cf.TypeLine,
                OracleText = cf.OracleText,
                Colors = cf.Colors,
                ColorIndicator = cf.ColorIndicator,
                Power = cf.Power,
                Toughness = cf.Toughness,
                Loyalty = cf.Loyalty,
                Defense = cf.Defense,
                FlavorText = cf.FlavorText,
                IllustrationId = cf.IllustrationId,
                ImageUris = cf.ImageUris is null ? null : new Domain.ValueObjects.CardImageUris
                {
                    Small = cf.ImageUris.Small,
                    Normal = cf.ImageUris.Normal,
                    Large = cf.ImageUris.Large,
                    Png = cf.ImageUris.Png,
                    ArtCrop = cf.ImageUris.ArtCrop,
                    BorderCrop = cf.ImageUris.BorderCrop
                },
                Cmc = cf.Cmc,
                OracleId = cf.OracleId,
                Layout = cf.Layout,
                PrintedName = cf.PrintedName,
                PrintedText = cf.PrintedText,
                PrintedTypeLine = cf.PrintedTypeLine,
                Watermark = cf.Watermark,
                Artist = cf.Artist,
                ArtistId = cf.ArtistId,
                FaceIndex = index
            }).ToList() ?? [],
            AllParts = sc.AllParts?.Select(ap => new RelatedCard
            {
                Id = Guid.NewGuid(),
                Object = ap.Object ?? "related_card",
                RelatedCardScryfallId = ap.Id,
                Component = ap.Component ?? string.Empty,
                Name = ap.Name ?? string.Empty,
                TypeLine = ap.TypeLine ?? string.Empty,
                Uri = ap.Uri ?? string.Empty
            }).ToList() ?? []
        };
    }

    // Scryfall card model for deserialization
    private record ScryfallCard
    {
        public Guid Id { get; init; }
        public Guid? OracleId { get; init; }
        public string? Object { get; init; }
        public string? Name { get; init; }
        public string? Lang { get; init; }
        public string? Layout { get; init; }
        public string? ManaCost { get; init; }
        public decimal Cmc { get; init; }
        public string? TypeLine { get; init; }
        public string? OracleText { get; init; }
        public string? Power { get; init; }
        public string? Toughness { get; init; }
        public string? Loyalty { get; init; }
        public string? Defense { get; init; }
        public List<string>? Colors { get; init; }
        public List<string>? ColorIdentity { get; init; }
        public List<string>? ColorIndicator { get; init; }
        public List<string>? ProducedMana { get; init; }
        public List<string>? Keywords { get; init; }
        public bool Reserved { get; init; }
        public int? EdhrecRank { get; init; }
        public int? PennyRank { get; init; }
        public string? Artist { get; init; }
        public List<Guid>? ArtistIds { get; init; }
        public bool Booster { get; init; }
        public string? BorderColor { get; init; }
        public Guid CardBackId { get; init; }
        public string? CollectorNumber { get; init; }
        public bool? ContentWarning { get; init; }
        public bool Digital { get; init; }
        public List<string>? Finishes { get; init; }
        public string? FlavorName { get; init; }
        public string? FlavorText { get; init; }
        public List<string>? FrameEffects { get; init; }
        public string? Frame { get; init; }
        public bool FullArt { get; init; }
        public List<string>? Games { get; init; }
        public bool HighresImage { get; init; }
        public Guid? IllustrationId { get; init; }
        public string? ImageStatus { get; init; }
        public bool Oversized { get; init; }
        public string? PrintedName { get; init; }
        public string? PrintedText { get; init; }
        public string? PrintedTypeLine { get; init; }
        public bool Promo { get; init; }
        public List<string>? PromoTypes { get; init; }
        public string? Rarity { get; init; }
        public DateTime ReleasedAt { get; init; }
        public bool Reprint { get; init; }
        public string? Set { get; init; }
        public Guid SetId { get; init; }
        public string? SetName { get; init; }
        public string? SetType { get; init; }
        public string? SetUri { get; init; }
        public string? SetSearchUri { get; init; }
        public string? ScryfallSetUri { get; init; }
        public bool StorySpotlight { get; init; }
        public bool Textless { get; init; }
        public bool Variation { get; init; }
        public Guid? VariationOf { get; init; }
        public string? SecurityStamp { get; init; }
        public string? Watermark { get; init; }
        public string? Uri { get; init; }
        public string? ScryfallUri { get; init; }
        public string? PrintsSearchUri { get; init; }
        public string? RulingsUri { get; init; }
        public int? ArenaId { get; init; }
        public int? MtgoId { get; init; }
        public int? MtgoFoilId { get; init; }
        public List<int>? MultiverseIds { get; init; }
        public int? TcgplayerId { get; init; }
        public int? TcgplayerEtchedId { get; init; }
        public int? CardmarketId { get; init; }
        public ScryfallImageUris? ImageUris { get; init; }
        public ScryfallPrices? Prices { get; init; }
        public ScryfallLegalities? Legalities { get; init; }
        public List<ScryfallCardFace>? CardFaces { get; init; }
        public List<ScryfallRelatedCard>? AllParts { get; init; }
    }

    private record ScryfallImageUris
    {
        public string? Small { get; init; }
        public string? Normal { get; init; }
        public string? Large { get; init; }
        public string? Png { get; init; }
        public string? ArtCrop { get; init; }
        public string? BorderCrop { get; init; }
    }

    private record ScryfallPrices
    {
        public string? Usd { get; init; }
        public string? UsdFoil { get; init; }
        public string? UsdEtched { get; init; }
        public string? Eur { get; init; }
        public string? EurFoil { get; init; }
        public string? EurEtched { get; init; }
        public string? Tix { get; init; }
    }

    private record ScryfallLegalities
    {
        public string? Standard { get; init; }
        public string? Future { get; init; }
        public string? Historic { get; init; }
        public string? Timeless { get; init; }
        public string? Gladiator { get; init; }
        public string? Pioneer { get; init; }
        public string? Explorer { get; init; }
        public string? Modern { get; init; }
        public string? Legacy { get; init; }
        public string? Pauper { get; init; }
        public string? Vintage { get; init; }
        public string? Penny { get; init; }
        public string? Commander { get; init; }
        public string? Oathbreaker { get; init; }
        public string? StandardBrawl { get; init; }
        public string? Brawl { get; init; }
        public string? Alchemy { get; init; }
        public string? PauperCommander { get; init; }
        public string? Duel { get; init; }
        public string? Oldschool { get; init; }
        public string? Premodern { get; init; }
        public string? Predh { get; init; }
    }

    private record ScryfallCardFace
    {
        public string? Object { get; init; }
        public string? Name { get; init; }
        public string? ManaCost { get; init; }
        public string? TypeLine { get; init; }
        public string? OracleText { get; init; }
        public List<string>? Colors { get; init; }
        public List<string>? ColorIndicator { get; init; }
        public string? Power { get; init; }
        public string? Toughness { get; init; }
        public string? Loyalty { get; init; }
        public string? Defense { get; init; }
        public string? FlavorText { get; init; }
        public Guid? IllustrationId { get; init; }
        public ScryfallImageUris? ImageUris { get; init; }
        public decimal? Cmc { get; init; }
        public Guid? OracleId { get; init; }
        public string? Layout { get; init; }
        public string? PrintedName { get; init; }
        public string? PrintedText { get; init; }
        public string? PrintedTypeLine { get; init; }
        public string? Watermark { get; init; }
        public string? Artist { get; init; }
        public Guid? ArtistId { get; init; }
    }

    private record ScryfallRelatedCard
    {
        public string? Object { get; init; }
        public Guid Id { get; init; }
        public string? Component { get; init; }
        public string? Name { get; init; }
        public string? TypeLine { get; init; }
        public string? Uri { get; init; }
    }
}
