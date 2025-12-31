using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OracleScry.Domain.Interfaces;
using OracleScry.Infrastructure.Persistence;
using OracleScry.Infrastructure.Persistence.Repositories;
using OracleScry.Infrastructure.Services;

namespace OracleScry.Infrastructure;

/// <summary>
/// Dependency injection extension for Infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add DbContext
        services.AddDbContext<OracleScryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(OracleScryDbContext).Assembly.FullName)));

        // Add repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<ICardImportRepository, CardImportRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add HTTP clients
        services.AddHttpClient<IScryfallApiClient, ScryfallApiClient>();

        return services;
    }
}
