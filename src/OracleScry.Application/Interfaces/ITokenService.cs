using OracleScry.Domain.Identity;

namespace OracleScry.Application.Interfaces;

/// <summary>
/// JWT token service interface.
/// </summary>
public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user, IList<string> roles);
    string GenerateRefreshToken();
    DateTime GetAccessTokenExpiration();
    DateTime GetRefreshTokenExpiration();
}
