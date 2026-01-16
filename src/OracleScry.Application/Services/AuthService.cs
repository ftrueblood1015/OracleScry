using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OracleScry.Application.DTOs.Auth;
using OracleScry.Application.Interfaces;
using OracleScry.Domain.Constants;
using OracleScry.Domain.Entities;
using OracleScry.Domain.Identity;
using OracleScry.Infrastructure.Persistence;

namespace OracleScry.Application.Services;

/// <summary>
/// Authentication service handling user registration, login, and token management.
/// </summary>
public class AuthService(
    UserManager<ApplicationUser> userManager,
    OracleScryDbContext context,
    ITokenService tokenService) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly OracleScryDbContext _context = context;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        // Check if user exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return AuthResponse.Failure("A user with this email already exists.");
        }

        // Create new user
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            DisplayName = request.DisplayName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return AuthResponse.Failure(errors);
        }

        // Assign default User role
        await _userManager.AddToRoleAsync(user, Roles.User);

        // Generate tokens
        return await GenerateTokensAsync(user, ct);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return AuthResponse.Failure("Invalid email or password.");
        }

        var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
        {
            return AuthResponse.Failure("Invalid email or password.");
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        // Generate tokens
        return await GenerateTokensAsync(user, ct);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct = default)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, ct);

        if (refreshToken == null || !refreshToken.IsActive)
        {
            return AuthResponse.Failure("Invalid or expired refresh token.");
        }

        // Revoke old token
        refreshToken.RevokedAt = DateTime.UtcNow;

        // Generate new tokens
        var response = await GenerateTokensAsync(refreshToken.User, ct);

        // Link old token to new one
        refreshToken.ReplacedByToken = response.RefreshToken;

        await _context.SaveChangesAsync(ct);

        return response;
    }

    public async Task RevokeTokenAsync(string token, CancellationToken ct = default)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, ct);

        if (refreshToken != null && refreshToken.IsActive)
        {
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<UserDto?> GetCurrentUserAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            user.CreatedAt,
            user.LastLoginAt,
            roles.ToList());
    }

    private async Task<AuthResponse> GenerateTokensAsync(ApplicationUser user, CancellationToken ct)
    {
        // Fetch user roles
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshTokenValue = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshTokenValue,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = _tokenService.GetRefreshTokenExpiration()
        };

        await _context.RefreshTokens.AddAsync(refreshToken, ct);
        await _context.SaveChangesAsync(ct);

        var userDto = new UserDto(
            user.Id,
            user.Email ?? string.Empty,
            user.DisplayName,
            user.CreatedAt,
            user.LastLoginAt,
            roles.ToList());

        return AuthResponse.Ok(
            accessToken,
            refreshTokenValue,
            _tokenService.GetAccessTokenExpiration(),
            userDto);
    }
}
