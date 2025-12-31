namespace OracleScry.Application.DTOs.Cards;

/// <summary>
/// DTO for card search/filter parameters.
/// </summary>
public class CardFilterDto
{
    public string? Query { get; set; }
    public string? SetCode { get; set; }
    public List<string>? Colors { get; set; }
    public string? Rarity { get; set; }
    public decimal? MinCmc { get; set; }
    public decimal? MaxCmc { get; set; }
    public string? TypeLine { get; set; }
    public string? Format { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "name";
    public bool SortDescending { get; set; } = false;

    // Ensure valid pagination values
    public int GetValidatedPage() => Math.Max(1, Page);
    public int GetValidatedPageSize() => Math.Clamp(PageSize, 1, 100);
}
