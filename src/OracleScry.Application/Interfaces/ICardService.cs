using OracleScry.Application.DTOs.Cards;
using OracleScry.Application.DTOs.Common;

namespace OracleScry.Application.Interfaces;

/// <summary>
/// Card service interface for card-related operations.
/// </summary>
public interface ICardService
{
    Task<CardDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<CardDto?> GetByScryfallIdAsync(Guid scryfallId, CancellationToken ct = default);
    Task<PagedResult<CardSummaryDto>> SearchAsync(CardFilterDto filter, CancellationToken ct = default);
    Task<IEnumerable<CardSummaryDto>> GetBySetAsync(string setCode, CancellationToken ct = default);
    Task<IEnumerable<string>> GetSetsAsync(CancellationToken ct = default);
    Task<IEnumerable<CardSummaryDto>> GetRandomCardsAsync(int count = 10, CancellationToken ct = default);
}
