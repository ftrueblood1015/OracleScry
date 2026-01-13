using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OracleScry.Application.DTOs.Decks;
using OracleScry.Application.Interfaces;

namespace OracleScry.Api.Controllers;

/// <summary>
/// API controller for deck management.
/// All endpoints require authentication and operate on the current user's decks.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DecksController(IDeckService deckService) : ControllerBase
{
    private readonly IDeckService _deckService = deckService;

    /// <summary>
    /// Get all decks for the current user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DeckSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMyDecks(CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var decks = await _deckService.GetUserDecksAsync(userId.Value, ct);
        return Ok(decks);
    }

    /// <summary>
    /// Get a specific deck by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DeckDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDeck(Guid id, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var deck = await _deckService.GetByIdAsync(id, userId.Value, ct);
        return deck is null ? NotFound() : Ok(deck);
    }

    /// <summary>
    /// Create a new deck.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DeckDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateDeck([FromBody] CreateDeckRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Deck name is required");

        var deck = await _deckService.CreateAsync(userId.Value, request, ct);
        return CreatedAtAction(nameof(GetDeck), new { id = deck.Id }, deck);
    }

    /// <summary>
    /// Update a deck.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(DeckDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateDeck(Guid id, [FromBody] UpdateDeckRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var deck = await _deckService.UpdateAsync(id, userId.Value, request, ct);
        return deck is null ? NotFound() : Ok(deck);
    }

    /// <summary>
    /// Delete a deck.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteDeck(Guid id, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var deleted = await _deckService.DeleteAsync(id, userId.Value, ct);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Add a card to a deck.
    /// </summary>
    [HttpPost("{id:guid}/cards")]
    [ProducesResponseType(typeof(DeckCardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddCard(Guid id, [FromBody] AddCardRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var card = await _deckService.AddCardAsync(id, userId.Value, request, ct);
        return card is null ? NotFound() : Ok(card);
    }

    /// <summary>
    /// Update a card in a deck (quantity, sideboard).
    /// </summary>
    [HttpPut("{deckId:guid}/cards/{cardId:guid}")]
    [ProducesResponseType(typeof(DeckCardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateCard(Guid deckId, Guid cardId, [FromBody] UpdateCardRequest request, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var card = await _deckService.UpdateCardAsync(deckId, cardId, userId.Value, request, ct);
        return card is null ? NotFound() : Ok(card);
    }

    /// <summary>
    /// Remove a card from a deck.
    /// </summary>
    [HttpDelete("{deckId:guid}/cards/{cardId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveCard(Guid deckId, Guid cardId, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var removed = await _deckService.RemoveCardAsync(deckId, cardId, userId.Value, ct);
        return removed ? NoContent() : NotFound();
    }

    /// <summary>
    /// Get deck statistics (mana curve, colors, purposes, etc.).
    /// </summary>
    [HttpGet("{id:guid}/stats")]
    [ProducesResponseType(typeof(DeckStatsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetStats(Guid id, CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var stats = await _deckService.GetStatsAsync(id, userId.Value, ct);
        return stats is null ? NotFound() : Ok(stats);
    }

    private Guid? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return null;
        return userId;
    }
}
