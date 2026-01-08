using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;

namespace OracleScry.Domain.Interfaces;

/// <summary>
/// Repository interface for CardPurpose entities.
/// </summary>
public interface ICardPurposeRepository : IRepository<CardPurpose>
{
    /// <summary>Get purpose by slug</summary>
    Task<CardPurpose?> GetBySlugAsync(string slug, CancellationToken ct = default);

    /// <summary>Get all active purposes ordered by display order</summary>
    Task<IReadOnlyList<CardPurpose>> GetAllActiveAsync(CancellationToken ct = default);

    /// <summary>Get purposes by category</summary>
    Task<IReadOnlyList<CardPurpose>> GetByCategoryAsync(PurposeCategory category, CancellationToken ct = default);

    /// <summary>Get purposes with their patterns for extraction</summary>
    Task<IReadOnlyList<CardPurpose>> GetWithPatternsAsync(CancellationToken ct = default);
}
