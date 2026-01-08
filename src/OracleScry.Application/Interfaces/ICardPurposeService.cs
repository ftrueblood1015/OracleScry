using OracleScry.Application.DTOs.Purposes;

namespace OracleScry.Application.Interfaces;

/// <summary>
/// Service for card purpose CRUD operations.
/// </summary>
public interface ICardPurposeService
{
    /// <summary>Get all active purposes</summary>
    Task<IReadOnlyList<CardPurposeSummaryDto>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Get purposes grouped by category</summary>
    Task<Dictionary<string, IReadOnlyList<CardPurposeSummaryDto>>> GetByCategoryAsync(CancellationToken ct = default);

    /// <summary>Get a purpose by ID</summary>
    Task<CardPurposeDto?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>Get a purpose by slug</summary>
    Task<CardPurposeDto?> GetBySlugAsync(string slug, CancellationToken ct = default);

    /// <summary>Create a new purpose</summary>
    Task<CardPurposeDto> CreateAsync(CreateCardPurposeRequest request, CancellationToken ct = default);

    /// <summary>Update an existing purpose</summary>
    Task<CardPurposeDto?> UpdateAsync(Guid id, UpdateCardPurposeRequest request, CancellationToken ct = default);

    /// <summary>Delete a purpose</summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);

    /// <summary>Get purposes assigned to a specific card</summary>
    Task<IReadOnlyList<CardPurposeAssignmentDto>> GetPurposesForCardAsync(Guid cardId, CancellationToken ct = default);
}
