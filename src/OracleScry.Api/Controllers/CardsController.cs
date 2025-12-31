using Microsoft.AspNetCore.Mvc;
using OracleScry.Application.DTOs.Cards;
using OracleScry.Application.Interfaces;

namespace OracleScry.Api.Controllers;

/// <summary>
/// Cards API controller for card-related operations.
/// All endpoints are publicly accessible (read-only).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CardsController(ICardService cardService) : ControllerBase
{
    private readonly ICardService _cardService = cardService;

    /// <summary>
    /// Search and filter cards with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Application.DTOs.Common.PagedResult<CardSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] CardFilterDto filter, CancellationToken ct)
    {
        var result = await _cardService.SearchAsync(filter, ct);
        return Ok(result);
    }

    /// <summary>
    /// Get card by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var card = await _cardService.GetByIdAsync(id, ct);
        return card is null ? NotFound() : Ok(card);
    }

    /// <summary>
    /// Get card by Scryfall ID.
    /// </summary>
    [HttpGet("scryfall/{scryfallId:guid}")]
    [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByScryfallId(Guid scryfallId, CancellationToken ct)
    {
        var card = await _cardService.GetByScryfallIdAsync(scryfallId, ct);
        return card is null ? NotFound() : Ok(card);
    }

    /// <summary>
    /// Get all available set codes.
    /// </summary>
    [HttpGet("sets")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSets(CancellationToken ct)
    {
        var sets = await _cardService.GetSetsAsync(ct);
        return Ok(sets);
    }

    /// <summary>
    /// Get all cards in a specific set.
    /// </summary>
    [HttpGet("sets/{setCode}")]
    [ProducesResponseType(typeof(IEnumerable<CardSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySet(string setCode, CancellationToken ct)
    {
        var cards = await _cardService.GetBySetAsync(setCode, ct);
        return Ok(cards);
    }

    /// <summary>
    /// Get random cards.
    /// </summary>
    [HttpGet("random")]
    [ProducesResponseType(typeof(IEnumerable<CardSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRandom([FromQuery] int count = 10, CancellationToken ct = default)
    {
        var cards = await _cardService.GetRandomCardsAsync(Math.Clamp(count, 1, 50), ct);
        return Ok(cards);
    }
}
