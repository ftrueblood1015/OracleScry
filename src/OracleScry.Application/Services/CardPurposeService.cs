using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using OracleScry.Application.DTOs.Purposes;
using OracleScry.Application.Interfaces;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Enums;
using OracleScry.Domain.Interfaces;
using OracleScry.Infrastructure.Persistence;

namespace OracleScry.Application.Services;

/// <summary>
/// Service for managing card purposes.
/// </summary>
public class CardPurposeService : ICardPurposeService
{
    private readonly OracleScryDbContext _context;
    private readonly ICardPurposeRepository _purposeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CardPurposeService(
        OracleScryDbContext context,
        ICardPurposeRepository purposeRepository,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _purposeRepository = purposeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CardPurposeSummaryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var purposes = await _purposeRepository.GetAllActiveAsync(ct);

        return purposes.Select(p => new CardPurposeSummaryDto(
            p.Id,
            p.Name,
            p.Slug,
            p.Description,
            p.Category.ToString()
        )).ToList();
    }

    public async Task<Dictionary<string, IReadOnlyList<CardPurposeSummaryDto>>> GetByCategoryAsync(CancellationToken ct = default)
    {
        var purposes = await _purposeRepository.GetAllActiveAsync(ct);

        return purposes
            .GroupBy(p => p.Category.ToString())
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyList<CardPurposeSummaryDto>)g.Select(p => new CardPurposeSummaryDto(
                    p.Id,
                    p.Name,
                    p.Slug,
                    p.Description,
                    p.Category.ToString()
                )).ToList()
            );
    }

    public async Task<CardPurposeDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var purpose = await _purposeRepository.GetByIdAsync(id, ct);
        return purpose != null ? MapToDto(purpose) : null;
    }

    public async Task<CardPurposeDto?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var purpose = await _purposeRepository.GetBySlugAsync(slug, ct);
        return purpose != null ? MapToDto(purpose) : null;
    }

    public async Task<CardPurposeDto> CreateAsync(CreateCardPurposeRequest request, CancellationToken ct = default)
    {
        if (!Enum.TryParse<PurposeCategory>(request.Category, true, out var category))
        {
            throw new ArgumentException($"Invalid category: {request.Category}");
        }

        var slug = GenerateSlug(request.Name);

        // Check for duplicate slug
        var existing = await _purposeRepository.GetBySlugAsync(slug, ct);
        if (existing != null)
        {
            throw new InvalidOperationException($"A purpose with slug '{slug}' already exists");
        }

        var purpose = new CardPurpose
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = slug,
            Description = request.Description,
            Category = category,
            DisplayOrder = request.DisplayOrder,
            IsActive = true,
            Patterns = request.Patterns
        };

        await _purposeRepository.AddAsync(purpose, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(purpose);
    }

    public async Task<CardPurposeDto?> UpdateAsync(Guid id, UpdateCardPurposeRequest request, CancellationToken ct = default)
    {
        var purpose = await _purposeRepository.GetByIdAsync(id, ct);
        if (purpose == null)
            return null;

        if (request.Name != null)
        {
            purpose.Name = request.Name;
            purpose.Slug = GenerateSlug(request.Name);
        }

        if (request.Description != null)
            purpose.Description = request.Description;

        if (request.Category != null && Enum.TryParse<PurposeCategory>(request.Category, true, out var category))
            purpose.Category = category;

        if (request.DisplayOrder.HasValue)
            purpose.DisplayOrder = request.DisplayOrder.Value;

        if (request.IsActive.HasValue)
            purpose.IsActive = request.IsActive.Value;

        if (request.Patterns != null)
            purpose.Patterns = request.Patterns;

        _purposeRepository.Update(purpose);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToDto(purpose);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var purpose = await _purposeRepository.GetByIdAsync(id, ct);
        if (purpose == null)
            return false;

        _purposeRepository.Remove(purpose);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<CardPurposeAssignmentDto>> GetPurposesForCardAsync(Guid cardId, CancellationToken ct = default)
    {
        var assignments = await _context.CardCardPurposes
            .AsNoTracking()
            .Include(ccp => ccp.CardPurpose)
            .Where(ccp => ccp.CardId == cardId)
            .OrderByDescending(ccp => ccp.Confidence)
            .ToListAsync(ct);

        return assignments.Select(a => new CardPurposeAssignmentDto(
            a.CardPurposeId,
            a.CardPurpose.Name,
            a.CardPurpose.Slug,
            a.CardPurpose.Category.ToString(),
            a.Confidence,
            a.MatchedPattern,
            a.AssignedAt,
            a.AssignedBy
        )).ToList();
    }

    private static CardPurposeDto MapToDto(CardPurpose purpose) => new(
        purpose.Id,
        purpose.Name,
        purpose.Slug,
        purpose.Description,
        purpose.Category.ToString(),
        purpose.DisplayOrder,
        purpose.IsActive,
        purpose.Patterns
    );

    private static string GenerateSlug(string name)
    {
        var slug = name.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        return slug.Trim('-');
    }
}
