namespace OracleScry.Application.DTOs.Auth;

/// <summary>
/// DTO for user information.
/// </summary>
public record UserDto(
    Guid Id,
    string Email,
    string? DisplayName,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);
