using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OracleScry.Application.DTOs.Common;
using OracleScry.Application.DTOs.Import;
using OracleScry.Application.Interfaces;

namespace OracleScry.Api.Controllers;

/// <summary>
/// Import API controller for viewing import history and statistics.
/// All endpoints require Admin role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Admin")]
public class ImportController(ICardImportService importService) : ControllerBase
{
    private readonly ICardImportService _importService = importService;

    /// <summary>
    /// Get paginated import history.
    /// </summary>
    [HttpGet("history")]
    [ProducesResponseType(typeof(PagedResult<CardImportSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _importService.GetHistoryAsync(
            Math.Max(1, page),
            Math.Clamp(pageSize, 1, 100),
            ct);
        return Ok(result);
    }

    /// <summary>
    /// Get single import details with errors.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CardImportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var import = await _importService.GetByIdAsync(id, ct);
        return import is null ? NotFound() : Ok(import);
    }

    /// <summary>
    /// Get the most recent import.
    /// </summary>
    [HttpGet("latest")]
    [ProducesResponseType(typeof(CardImportSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLatest(CancellationToken ct)
    {
        var import = await _importService.GetLatestAsync(ct);
        return import is null ? NotFound() : Ok(import);
    }

    /// <summary>
    /// Get aggregated import statistics.
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ImportStatsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var stats = await _importService.GetStatsAsync(ct);
        return Ok(stats);
    }

    /// <summary>
    /// Check if an import is currently running.
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatus(CancellationToken ct)
    {
        var isRunning = await _importService.IsImportRunningAsync(ct);
        return Ok(new { isRunning });
    }
}
