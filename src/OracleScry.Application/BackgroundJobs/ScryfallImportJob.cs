using Microsoft.Extensions.Logging;
using OracleScry.Application.Interfaces;

namespace OracleScry.Application.BackgroundJobs;

/// <summary>
/// Hangfire job for scheduled Scryfall imports.
/// Runs daily at 3:00 AM UTC.
/// </summary>
public class ScryfallImportJob
{
    private readonly ICardImportService _importService;
    private readonly ILogger<ScryfallImportJob> _logger;

    public ScryfallImportJob(ICardImportService importService, ILogger<ScryfallImportJob> logger)
    {
        _importService = importService;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken ct)
    {
        _logger.LogInformation("Starting scheduled Scryfall import job");

        try
        {
            // Check if import is already running
            if (await _importService.IsImportRunningAsync(ct))
            {
                _logger.LogWarning("Import already running, skipping scheduled run");
                return;
            }

            var result = await _importService.ExecuteImportAsync(ct);

            _logger.LogInformation(
                "Scheduled import completed: {Added} added, {Updated} updated, {Failed} failed",
                result.CardsAdded, result.CardsUpdated, result.CardsFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Scheduled import job failed");
            throw; // Re-throw to mark job as failed in Hangfire
        }
    }
}
