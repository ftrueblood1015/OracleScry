using OracleScry.Application.DTOs.Decks;

namespace OracleScry.Application.Interfaces;

/// <summary>
/// Service interface for deck management operations.
/// All operations require a userId for ownership validation.
/// </summary>
public interface IDeckService
{
    // Deck CRUD
    Task<IEnumerable<DeckSummaryDto>> GetUserDecksAsync(Guid userId, CancellationToken ct = default);
    Task<DeckDto?> GetByIdAsync(Guid deckId, Guid userId, CancellationToken ct = default);
    Task<DeckDto> CreateAsync(Guid userId, CreateDeckRequest request, CancellationToken ct = default);
    Task<DeckDto?> UpdateAsync(Guid deckId, Guid userId, UpdateDeckRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid deckId, Guid userId, CancellationToken ct = default);

    // Card Management
    Task<DeckCardDto?> AddCardAsync(Guid deckId, Guid userId, AddCardRequest request, CancellationToken ct = default);
    Task<DeckCardDto?> UpdateCardAsync(Guid deckId, Guid cardId, Guid userId, UpdateCardRequest request, CancellationToken ct = default);
    Task<bool> RemoveCardAsync(Guid deckId, Guid cardId, Guid userId, CancellationToken ct = default);

    // Statistics
    Task<DeckStatsDto?> GetStatsAsync(Guid deckId, Guid userId, CancellationToken ct = default);
}
