using OracleScry.Domain.Interfaces;

namespace OracleScry.Infrastructure.Persistence.Repositories;

/// <summary>
/// Unit of Work implementation coordinating repository operations.
/// Ensures all changes are committed in a single transaction.
/// </summary>
public class UnitOfWork(OracleScryDbContext context) : IUnitOfWork
{
    private readonly OracleScryDbContext _context = context;
    private ICardRepository? _cards;
    private bool _disposed;

    public ICardRepository Cards => _cards ??= new CardRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }
}
