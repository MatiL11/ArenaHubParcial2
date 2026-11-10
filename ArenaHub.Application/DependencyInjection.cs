using ArenaHub.Application.Equipos;
using ArenaHub.Application.Inscripciones;
using ArenaHub.Application.Jugadores;
using ArenaHub.Application.Partidas;
using ArenaHub.Application.Ranking;
using ArenaHub.Application.Torneos;
using Microsoft.Extensions.DependencyInjection;

namespace ArenaHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) =>
        services
            .AddScoped<JugadoresService>()
            .AddScoped<EquiposService>()
            .AddScoped<TorneosService>()
            .AddScoped<InscripcionesService>()
            .AddScoped<PartidasService>()
            .AddScoped<RankingService>();
}
