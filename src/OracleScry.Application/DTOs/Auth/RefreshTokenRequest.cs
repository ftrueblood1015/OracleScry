using System.ComponentModel.DataAnnotations;

namespace OracleScry.Application.DTOs.Auth;

/// <summary>
/// DTO for refresh token request.
/// </summary>
public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
