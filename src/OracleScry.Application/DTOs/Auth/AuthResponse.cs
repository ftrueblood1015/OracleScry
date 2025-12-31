namespace OracleScry.Application.DTOs.Auth;

/// <summary>
/// DTO for authentication response containing tokens.
/// </summary>
public record AuthResponse(
    bool Success,
    string? AccessToken,
    string? RefreshToken,
    DateTime? ExpiresAt,
    UserDto? User,
    string? Error
)
{
    public static AuthResponse Failure(string error) => new(false, null, null, null, null, error);
    public static AuthResponse Ok(string accessToken, string refreshToken, DateTime expiresAt, UserDto user)
        => new(true, accessToken, refreshToken, expiresAt, user, null);
}
