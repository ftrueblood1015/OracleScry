using Microsoft.Extensions.Logging;
using OracleScry.Application.Interfaces;

namespace OracleScry.Application.BackgroundJobs;

/// <summary>
/// Hangfire job for scheduled purpose extraction.
/// Runs after card imports to categorize new cards.
/// </summary>
public class PurposeExtractionJob
{
    private readonly IPurposeExtractionService _extractionService;
    private readonly ILogger<PurposeExtractionJob> _logger;

    public PurposeExtractionJob(
        IPurposeExtractionService extractionService,
        ILogger<PurposeExtractionJob> logger)
    {
        _extractionService = extractionService;
        _logger = logger;
    }

    /// <summary>
    /// Execute purpose extraction for cards without purposes.
    /// </summary>
    public async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting scheduled purpose extraction job");

        try
        {
            if (await _extractionService.IsExtractionRunningAsync(ct))
            {
                _logger.LogWarning("Extraction already running, skipping scheduled run");
                return;
            }

            var result = await _extractionService.ExecuteExtractionAsync(reprocessAll: false, ct);

            _logger.LogInformation(
                "Scheduled extraction completed: {Processed} processed, {Assigned} purposes assigned, {Errors} errors",
                result.ProcessedCards, result.PurposesAssigned, result.ErrorCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Scheduled extraction job failed");
            throw;
        }
    }

    /// <summary>
    /// Execute full reprocessing of all cards.
    /// </summary>
    public async Task ExecuteFullReprocessAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting full purpose re-extraction job");

        try
        {
            if (await _extractionService.IsExtractionRunningAsync(ct))
            {
                _logger.LogWarning("Extraction already running, skipping full reprocess");
                return;
            }

            var result = await _extractionService.ExecuteExtractionAsync(reprocessAll: true, ct);

            _logger.LogInformation(
                "Full re-extraction completed: {Processed} processed, {Assigned} purposes assigned, {Errors} errors",
                result.ProcessedCards, result.PurposesAssigned, result.ErrorCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Full re-extraction job failed");
            throw;
        }
    }
}
