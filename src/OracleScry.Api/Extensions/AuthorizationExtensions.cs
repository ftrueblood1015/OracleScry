using OracleScry.Domain.Constants;

namespace OracleScry.Api.Extensions;

/// <summary>
/// Authorization policy configuration extensions.
/// </summary>
public static class AuthorizationExtensions
{
    /// <summary>
    /// Adds authorization policies for role-based access control.
    /// </summary>
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy =>
                policy.RequireRole(Roles.Admin))
            .AddPolicy("Authenticated", policy =>
                policy.RequireAuthenticatedUser());

        return services;
    }
}
