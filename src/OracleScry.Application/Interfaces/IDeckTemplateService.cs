using OracleScry.Application.DTOs.Decks;
using OracleScry.Application.DTOs.DeckTemplates;

namespace OracleScry.Application.Interfaces;

/// <summary>
/// Service interface for deck template operations.
/// Templates are public/read-only. Creating a deck from a template requires authentication.
/// </summary>
public interface IDeckTemplateService
{
    /// <summary>
    /// Get all active templates with optional filtering.
    /// </summary>
    /// <param name="format">Optional format filter (e.g., "Commander", "Standard")</param>
    /// <param name="search">Optional search term for name/description</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of template summaries</returns>
    Task<IEnumerable<DeckTemplateSummaryDto>> GetTemplatesAsync(
        string? format = null,
        string? search = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get a template by ID with all card details.
    /// </summary>
    /// <param name="templateId">Template ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Full template DTO or null if not found</returns>
    Task<DeckTemplateDto?> GetByIdAsync(Guid templateId, CancellationToken ct = default);

    /// <summary>
    /// Create a new deck by copying a template.
    /// </summary>
    /// <param name="userId">User ID who will own the new deck</param>
    /// <param name="templateId">Template ID to copy from</param>
    /// <param name="request">Deck creation options</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The newly created deck</returns>
    Task<DeckDto?> CreateDeckFromTemplateAsync(
        Guid userId,
        Guid templateId,
        CreateDeckFromTemplateRequest request,
        CancellationToken ct = default);
}
