using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Equipos;
using ArenaHub.Domain.Jugadores;
using ArenaHub.Domain.Torneos;
using Microsoft.EntityFrameworkCore;

namespace ArenaHub.Infrastructure.Persistencia;

public sealed class ArenaHubDbContext(DbContextOptions<ArenaHubDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Jugador> Jugadores => Set<Jugador>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Torneo> Torneos => Set<Torneo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ArenaHubDbContext).Assembly);
}
