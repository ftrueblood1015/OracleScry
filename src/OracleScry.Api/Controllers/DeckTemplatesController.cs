using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OracleScry.Application.DTOs.Decks;
using OracleScry.Application.DTOs.DeckTemplates;
using OracleScry.Application.Interfaces;

namespace OracleScry.Api.Controllers;

/// <summary>
/// API controller for deck templates (preconstructed decks).
/// Browse templates publicly; creating a deck from a template requires authentication.
/// </summary>
[ApiController]
[Route("api/deck-templates")]
public class DeckTemplatesController(IDeckTemplateService templateService) : ControllerBase
{
    private readonly IDeckTemplateService _templateService = templateService;

    /// <summary>
    /// Get all available deck templates with optional filtering.
    /// </summary>
    /// <param name="format">Filter by format (Commander, Standard, etc.)</param>
    /// <param name="search">Search in name, description, or set name</param>
    /// <param name="ct">Cancellation token</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DeckTemplateSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTemplates(
        [FromQuery] string? format,
        [FromQuery] string? search,
        CancellationToken ct)
    {
        var templates = await _templateService.GetTemplatesAsync(format, search, ct);
        return Ok(templates);
    }

    /// <summary>
    /// Get a specific deck template by ID with all card details.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DeckTemplateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTemplate(Guid id, CancellationToken ct)
    {
        var template = await _templateService.GetByIdAsync(id, ct);
        return template is null ? NotFound() : Ok(template);
    }

    /// <summary>
    /// Create a new deck by copying a template.
    /// The new deck will be owned by the authenticated user.
    /// </summary>
    [HttpPost("{id:guid}/create-deck")]
    [Authorize]
    [ProducesResponseType(typeof(DeckDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateDeckFromTemplate(
        Guid id,
        [FromBody] CreateDeckFromTemplateRequest request,
        CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Deck name is required");

        var deck = await _templateService.CreateDeckFromTemplateAsync(userId.Value, id, request, ct);

        if (deck is null)
            return NotFound("Template not found");

        return CreatedAtAction(
            actionName: "GetDeck",
            controllerName: "Decks",
            routeValues: new { id = deck.Id },
            value: deck);
    }

    private Guid? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return null;
        return userId;
    }
}
