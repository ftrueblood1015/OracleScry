using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OracleScry.Application.DTOs.Common;
using OracleScry.Application.DTOs.Purposes;
using OracleScry.Application.Interfaces;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;
using OracleScry.Domain.Interfaces;
using OracleScry.Infrastructure.Persistence;

namespace OracleScry.Application.Services;

/// <summary>
/// Service for extracting and assigning purposes to cards.
/// Uses batch processing for efficient database operations.
/// </summary>
public class PurposeExtractionService : IPurposeExtractionService
{
    private readonly OracleScryDbContext _context;
    private readonly IPurposeExtractionJobRepository _jobRepository;
    private readonly ICardPurposeRepository _purposeRepository;
    private readonly IPurposeExtractor _extractor;
    private readonly ILogger<PurposeExtractionService> _logger;
    private const int BatchSize = 500;

    public PurposeExtractionService(
        OracleScryDbContext context,
        IPurposeExtractionJobRepository jobRepository,
        ICardPurposeRepository purposeRepository,
        IPurposeExtractor extractor,
        ILogger<PurposeExtractionService> logger)
    {
        _context = context;
        _jobRepository = jobRepository;
        _purposeRepository = purposeRepository;
        _extractor = extractor;
        _logger = logger;
    }

    public async Task<PurposeExtractionJobDto> ExecuteExtractionAsync(bool reprocessAll = false, CancellationToken ct = default)
    {
        _logger.LogInformation("Starting purpose extraction. ReprocessAll: {ReprocessAll}", reprocessAll);

        if (await _jobRepository.HasRunningJobAsync(ct))
        {
            throw new InvalidOperationException("A purpose extraction job is already running");
        }

        // Create job record
        var job = new PurposeExtractionJob
        {
            Id = Guid.NewGuid(),
            StartedAt = DateTime.UtcNow,
            Status = ExtractionJobStatus.Pending,
            ReprocessAll = reprocessAll
        };

        await _jobRepository.AddAsync(job, ct);
        await _context.SaveChangesAsync(ct);

        try
        {
            job.Status = ExtractionJobStatus.Running;
            await _context.SaveChangesAsync(ct);

            // Get all purposes with patterns
            var purposes = await _purposeRepository.GetWithPatternsAsync(ct);
            if (purposes.Count == 0)
            {
                throw new InvalidOperationException("No purposes with patterns defined. Please create purposes first.");
            }

            _logger.LogInformation("Found {Count} purposes with patterns", purposes.Count);

            // Build query for cards to process
            IQueryable<Card> cardsQuery = _context.Cards
                .Include(c => c.CardFaces)
                .AsNoTracking();

            if (!reprocessAll)
            {
                // Only process cards that don't already have purposes assigned
                var cardsWithPurposes = _context.CardCardPurposes.Select(ccp => ccp.CardId).Distinct();
                cardsQuery = cardsQuery.Where(c => !cardsWithPurposes.Contains(c.Id));
            }

            job.TotalCards = await cardsQuery.CountAsync(ct);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Processing {Count} cards", job.TotalCards);

            // Process in batches
            var processed = 0;
            var purposesAssigned = 0;
            var errors = 0;

            var cardIds = await cardsQuery.Select(c => c.Id).ToListAsync(ct);

            foreach (var batch in cardIds.Chunk(BatchSize))
            {
                ct.ThrowIfCancellationRequested();

                var cards = await _context.Cards
                    .Include(c => c.CardFaces)
                    .Where(c => batch.Contains(c.Id))
                    .ToListAsync(ct);

                // If reprocessing, clear existing assignments for these cards
                if (reprocessAll)
                {
                    var existingAssignments = await _context.CardCardPurposes
                        .Where(ccp => batch.Contains(ccp.CardId))
                        .ToListAsync(ct);

                    _context.CardCardPurposes.RemoveRange(existingAssignments);
                }

                foreach (var card in cards)
                {
                    try
                    {
                        var matches = _extractor.ExtractPurposes(card, purposes);

                        foreach (var match in matches)
                        {
                            var assignment = new CardCardPurpose
                            {
                                CardId = card.Id,
                                CardPurposeId = match.Purpose.Id,
                                Confidence = match.Confidence,
                                MatchedPattern = match.MatchedPattern.Length > 500
                                    ? match.MatchedPattern[..500]
                                    : match.MatchedPattern,
                                AssignedAt = DateTime.UtcNow,
                                AssignedBy = "PurposeExtraction"
                            };

                            _context.CardCardPurposes.Add(assignment);
                            purposesAssigned++;
                        }

                        processed++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error extracting purposes for card {CardId}", card.Id);
                        errors++;
                        processed++;
                    }
                }

                await _context.SaveChangesAsync(ct);

                // Update progress
                job.ProcessedCards = processed;
                job.PurposesAssigned = purposesAssigned;
                job.ErrorCount = errors;
                await _context.SaveChangesAsync(ct);

                _logger.LogDebug("Processed {Processed}/{Total} cards", processed, job.TotalCards);
            }

            job.Status = ExtractionJobStatus.Completed;
            job.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation(
                "Purpose extraction completed. Processed: {Processed}, Assigned: {Assigned}, Errors: {Errors}",
                processed, purposesAssigned, errors);

            return MapToDto(job);
        }
        catch (OperationCanceledException)
        {
            job.Status = ExtractionJobStatus.Cancelled;
            job.CompletedAt = DateTime.UtcNow;
            job.ErrorMessage = "Operation was cancelled";
            await _context.SaveChangesAsync(CancellationToken.None);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Purpose extraction failed");

            job.Status = ExtractionJobStatus.Failed;
            job.CompletedAt = DateTime.UtcNow;
            job.ErrorMessage = ex.Message.Length > 2000 ? ex.Message[..2000] : ex.Message;
            await _context.SaveChangesAsync(CancellationToken.None);

            throw;
        }
    }

    public async Task<PagedResult<PurposeExtractionJobSummaryDto>> GetHistoryAsync(int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var jobs = await _jobRepository.GetHistoryAsync(page, pageSize, ct);
        var total = await _jobRepository.GetCountAsync(ct);

        var items = jobs.Select(j => new PurposeExtractionJobSummaryDto(
            j.Id,
            j.StartedAt,
            j.CompletedAt,
            j.Status.ToString(),
            j.ProcessedCards,
            j.PurposesAssigned
        )).ToList();

        return new PagedResult<PurposeExtractionJobSummaryDto>
        {
            Items = items,
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PurposeExtractionJobDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var job = await _jobRepository.GetByIdAsync(id, ct);
        return job != null ? MapToDto(job) : null;
    }

    public async Task<PurposeExtractionJobSummaryDto?> GetLatestAsync(CancellationToken ct = default)
    {
        var job = await _jobRepository.GetLatestAsync(ct);
        return job != null
            ? new PurposeExtractionJobSummaryDto(
                job.Id,
                job.StartedAt,
                job.CompletedAt,
                job.Status.ToString(),
                job.ProcessedCards,
                job.PurposesAssigned)
            : null;
    }

    public async Task<ExtractionStatsDto> GetStatsAsync(CancellationToken ct = default)
    {
        var jobs = await _context.PurposeExtractionJobs.ToListAsync(ct);

        var totalJobs = jobs.Count;
        var successfulJobs = jobs.Count(j => j.Status == ExtractionJobStatus.Completed);
        var failedJobs = jobs.Count(j => j.Status == ExtractionJobStatus.Failed);
        var totalProcessed = jobs.Sum(j => j.ProcessedCards);
        var totalAssigned = jobs.Sum(j => j.PurposesAssigned);
        var lastExtraction = jobs.MaxBy(j => j.StartedAt)?.CompletedAt;

        var cardsWithPurposes = await _context.CardCardPurposes
            .Select(ccp => ccp.CardId)
            .Distinct()
            .CountAsync(ct);

        var totalCards = await _context.Cards.CountAsync(ct);

        return new ExtractionStatsDto(
            totalJobs,
            successfulJobs,
            failedJobs,
            totalProcessed,
            totalAssigned,
            lastExtraction,
            cardsWithPurposes,
            totalCards - cardsWithPurposes
        );
    }

    public async Task<bool> IsExtractionRunningAsync(CancellationToken ct = default)
        => await _jobRepository.HasRunningJobAsync(ct);

    private static PurposeExtractionJobDto MapToDto(PurposeExtractionJob job) => new(
        job.Id,
        job.StartedAt,
        job.CompletedAt,
        job.Status.ToString(),
        job.TotalCards,
        job.ProcessedCards,
        job.PurposesAssigned,
        job.ErrorCount,
        job.ErrorMessage,
        job.ReprocessAll,
        job.CompletedAt.HasValue
            ? (job.CompletedAt.Value - job.StartedAt).TotalSeconds
            : null
    );
}
