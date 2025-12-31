using Microsoft.EntityFrameworkCore;
using OracleScry.Domain.Interfaces;

namespace OracleScry.Infrastructure.Persistence.Repositories;

/// <summary>
/// Generic repository implementation for basic CRUD operations.
/// Uses EF Core DbContext for data access.
/// </summary>
public class Repository<T>(OracleScryDbContext context) : IRepository<T> where T : class
{
    protected readonly OracleScryDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _dbSet.FindAsync([id], ct);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _dbSet.AddAsync(entity, ct);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Remove(T entity)
        => _dbSet.Remove(entity);
}
