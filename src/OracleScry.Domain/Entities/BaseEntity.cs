namespace OracleScry.Domain.Entities;

/// <summary>
/// Base entity with common properties for all domain entities.
/// Provides Id, ImportedOn, and LastUpdatedOn tracking.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime ImportedOn { get; set; }
    public DateTime LastUpdatedOn { get; set; }
}
