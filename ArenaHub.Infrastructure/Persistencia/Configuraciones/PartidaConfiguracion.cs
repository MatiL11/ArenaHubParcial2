using ArenaHub.Domain.Equipos;
using ArenaHub.Domain.Torneos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArenaHub.Infrastructure.Persistencia.Configuraciones;

internal sealed class PartidaConfiguracion : IEntityTypeConfiguration<Partida>
{
    public void Configure(EntityTypeBuilder<Partida> builder)
    {
        builder.HasOne<Equipo>().WithMany().HasForeignKey(p => p.EquipoAId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Equipo>().WithMany().HasForeignKey(p => p.EquipoBId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Equipo>().WithMany().HasForeignKey(p => p.GanadorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(p => new { p.TorneoId, p.Ronda, p.Numero }).IsUnique();
    }
}
