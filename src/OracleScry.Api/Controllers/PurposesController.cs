using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OracleScry.Application.DTOs.Common;
using OracleScry.Application.DTOs.Purposes;
using OracleScry.Application.Interfaces;

namespace OracleScry.Api.Controllers;

/// <summary>
/// API controller for card purposes and purpose extraction operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PurposesController(
    ICardPurposeService purposeService,
    IPurposeExtractionService extractionService) : ControllerBase
{
    private readonly ICardPurposeService _purposeService = purposeService;
    private readonly IPurposeExtractionService _extractionService = extractionService;

    #region Purpose CRUD

    /// <summary>
    /// Get all active purposes.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CardPurposeSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var purposes = await _purposeService.GetAllAsync(ct);
        return Ok(purposes);
    }

    /// <summary>
    /// Get purposes grouped by category.
    /// </summary>
    [HttpGet("by-category")]
    [ProducesResponseType(typeof(Dictionary<string, IReadOnlyList<CardPurposeSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategory(CancellationToken ct)
    {
        var grouped = await _purposeService.GetByCategoryAsync(ct);
        return Ok(grouped);
    }

    /// <summary>
    /// Get a purpose by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CardPurposeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var purpose = await _purposeService.GetByIdAsync(id, ct);
        return purpose is null ? NotFound() : Ok(purpose);
    }

    /// <summary>
    /// Get a purpose by slug.
    /// </summary>
    [HttpGet("by-slug/{slug}")]
    [ProducesResponseType(typeof(CardPurposeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
    {
        var purpose = await _purposeService.GetBySlugAsync(slug, ct);
        return purpose is null ? NotFound() : Ok(purpose);
    }

    /// <summary>
    /// Create a new purpose.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(CardPurposeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCardPurposeRequest request, CancellationToken ct)
    {
        try
        {
            var purpose = await _purposeService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = purpose.Id }, purpose);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing purpose.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(CardPurposeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCardPurposeRequest request, CancellationToken ct)
    {
        try
        {
            var purpose = await _purposeService.UpdateAsync(id, request, ct);
            return purpose is null ? NotFound() : Ok(purpose);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a purpose.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _purposeService.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Get purposes assigned to a specific card.
    /// </summary>
    [HttpGet("for-card/{cardId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<CardPurposeAssignmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPurposesForCard(Guid cardId, CancellationToken ct)
    {
        var purposes = await _purposeService.GetPurposesForCardAsync(cardId, ct);
        return Ok(purposes);
    }

    #endregion

    #region Extraction Jobs

    /// <summary>
    /// Trigger a purpose extraction job.
    /// </summary>
    [HttpPost("extraction/trigger")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(PurposeExtractionJobDto), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> TriggerExtraction(
        [FromQuery] bool reprocessAll = false,
        CancellationToken ct = default)
    {
        try
        {
            var job = await _extractionService.ExecuteExtractionAsync(reprocessAll, ct);
            return AcceptedAtAction(nameof(GetExtractionJob), new { id = job.Id }, job);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get paginated extraction job history.
    /// </summary>
    [HttpGet("extraction/history")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(PagedResult<PurposeExtractionJobSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExtractionHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _extractionService.GetHistoryAsync(
            Math.Max(1, page),
            Math.Clamp(pageSize, 1, 100),
            ct);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific extraction job.
    /// </summary>
    [HttpGet("extraction/{id:guid}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(PurposeExtractionJobDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExtractionJob(Guid id, CancellationToken ct)
    {
        var job = await _extractionService.GetByIdAsync(id, ct);
        return job is null ? NotFound() : Ok(job);
    }

    /// <summary>
    /// Get the most recent extraction job.
    /// </summary>
    [HttpGet("extraction/latest")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(PurposeExtractionJobSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLatestExtraction(CancellationToken ct)
    {
        var job = await _extractionService.GetLatestAsync(ct);
        return job is null ? NotFound() : Ok(job);
    }

    /// <summary>
    /// Get extraction statistics.
    /// </summary>
    [HttpGet("extraction/stats")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(ExtractionStatsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExtractionStats(CancellationToken ct)
    {
        var stats = await _extractionService.GetStatsAsync(ct);
        return Ok(stats);
    }

    /// <summary>
    /// Check if an extraction is currently running.
    /// </summary>
    [HttpGet("extraction/status")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExtractionStatus(CancellationToken ct)
    {
        var isRunning = await _extractionService.IsExtractionRunningAsync(ct);
        return Ok(new { isRunning });
    }

    #endregion
}
