using OracleScry.Domain.Entities;

namespace OracleScry.Domain.Interfaces;

/// <summary>
/// Card-specific repository interface with custom query methods.
/// Extends the generic repository with card-specific operations.
/// </summary>
public interface ICardRepository : IRepository<Card>
{
    Task<Card?> GetByScryfallIdAsync(Guid scryfallId, CancellationToken ct = default);
    Task<Card?> GetByIdWithFacesAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Card>> GetBySetCodeAsync(string setCode, CancellationToken ct = default);
    Task<IReadOnlyList<Card>> GetByOracleIdAsync(Guid oracleId, CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetDistinctSetCodesAsync(CancellationToken ct = default);
    Task<int> GetCountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Card>> GetRandomAsync(int count, CancellationToken ct = default);
    Task<bool> ExistsByScryfallIdAsync(Guid scryfallId, CancellationToken ct = default);
}
