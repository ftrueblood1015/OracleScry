using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OracleScry.Domain.Constants;
using OracleScry.Domain.Identity;

namespace OracleScry.Infrastructure.Identity;

/// <summary>
/// Seeds roles and admin user on application startup.
/// </summary>
public class RoleSeeder(
    RoleManager<IdentityRole<Guid>> roleManager,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    ILogger<RoleSeeder> logger)
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<RoleSeeder> _logger = logger;

    /// <summary>
    /// Seeds all roles and the admin user from configuration.
    /// </summary>
    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        foreach (var role in Roles.All)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                if (result.Succeeded)
                {
                    _logger.LogInformation("Created role: {Role}", role);
                }
                else
                {
                    _logger.LogError("Failed to create role {Role}: {Errors}",
                        role, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        var adminConfig = _configuration.GetSection("AdminUser");
        var adminEmail = adminConfig["Email"];
        var adminPassword = adminConfig["Password"];
        var displayName = adminConfig["DisplayName"] ?? "System Administrator";

        if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
        {
            _logger.LogWarning("Admin user not configured. Skipping admin seeding.");
            return;
        }

        if (adminPassword == "OVERRIDE_IN_ENV")
        {
            _logger.LogWarning("Admin password is placeholder. Set AdminUser__Password environment variable.");
            return;
        }

        var admin = await _userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                DisplayName = displayName,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(admin, adminPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Created admin user: {Email}", adminEmail);
            }
            else
            {
                _logger.LogError("Failed to create admin user: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return;
            }
        }

        // Ensure admin has Admin role
        if (!await _userManager.IsInRoleAsync(admin, Roles.Admin))
        {
            var result = await _userManager.AddToRoleAsync(admin, Roles.Admin);
            if (result.Succeeded)
            {
                _logger.LogInformation("Assigned Admin role to user: {Email}", adminEmail);
            }
            else
            {
                _logger.LogError("Failed to assign Admin role: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // Ensure admin also has User role
        if (!await _userManager.IsInRoleAsync(admin, Roles.User))
        {
            await _userManager.AddToRoleAsync(admin, Roles.User);
        }
    }
}
