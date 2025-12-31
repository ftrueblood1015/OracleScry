using Microsoft.AspNetCore.Identity;

namespace OracleScry.Domain.Identity;

/// <summary>
/// Application user entity extending ASP.NET Core Identity.
/// Custom properties for OracleScry user management.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string? DisplayName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }

    // Navigation properties for future phases
    // public ICollection<Deck> Decks { get; set; } = [];
    // public ICollection<UserCardCollection> Collection { get; set; } = [];
}
