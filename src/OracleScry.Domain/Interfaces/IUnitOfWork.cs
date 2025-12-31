namespace OracleScry.Domain.Interfaces;

/// <summary>
/// Unit of Work pattern interface for coordinating repository operations.
/// Ensures all changes are committed in a single transaction.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    ICardRepository Cards { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
