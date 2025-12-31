namespace OracleScry.Domain.Entities;

/// <summary>
/// Records an individual card import failure.
/// Links to the CardImport run where the error occurred.
/// </summary>
public class CardImportError : BaseEntity
{
    /// <summary>Foreign key to the import run</summary>
    public Guid CardImportId { get; set; }

    /// <summary>Navigation to parent import</summary>
    public CardImport CardImport { get; set; } = null!;

    /// <summary>Oracle ID of the card that failed (if available)</summary>
    public Guid? OracleId { get; set; }

    /// <summary>Name of the card that failed (if available)</summary>
    public string? CardName { get; set; }

    /// <summary>Error message describing what went wrong</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Stack trace for debugging (optional)</summary>
    public string? StackTrace { get; set; }
}
