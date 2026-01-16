namespace OracleScry.Domain.Constants;

/// <summary>
/// Role name constants for authorization.
/// </summary>
public static class Roles
{
    /// <summary>
    /// Administrator role with full access.
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// Standard user role.
    /// </summary>
    public const string User = "User";

    /// <summary>
    /// All available roles.
    /// </summary>
    public static readonly IReadOnlyList<string> All = [Admin, User];
}
