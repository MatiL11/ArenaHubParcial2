using ArenaHub.Application.Abstractions;
using ArenaHub.Infrastructure.Persistencia;
using ArenaHub.Infrastructure.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArenaHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString) =>
        services
            .AddDbContext<ArenaHubDbContext>(options => options.UseSqlite(connectionString))
            .AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ArenaHubDbContext>())
            .AddScoped<IJugadorRepository, JugadorRepository>()
            .AddScoped<IEquipoRepository, EquipoRepository>()
            .AddScoped<ITorneoRepository, TorneoRepository>();

    // Crea la base o la actualiza a la última migración.
    public static void AplicarMigraciones(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ArenaHubDbContext>().Database.Migrate();
    }
}
