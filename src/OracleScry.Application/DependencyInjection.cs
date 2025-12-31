using Microsoft.Extensions.DependencyInjection;
using OracleScry.Application.BackgroundJobs;
using OracleScry.Application.Interfaces;
using OracleScry.Application.Services;

namespace OracleScry.Application;

/// <summary>
/// Dependency injection extension for Application layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICardImportService, CardImportService>();
        services.AddScoped<ScryfallImportJob>();

        return services;
    }
}
