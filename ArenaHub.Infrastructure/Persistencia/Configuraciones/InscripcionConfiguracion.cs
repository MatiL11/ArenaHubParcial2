using ArenaHub.Domain.Equipos;
using ArenaHub.Domain.Torneos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArenaHub.Infrastructure.Persistencia.Configuraciones;

internal sealed class InscripcionConfiguracion : IEntityTypeConfiguration<Inscripcion>
{
    public void Configure(EntityTypeBuilder<Inscripcion> builder)
    {
        builder.HasOne(i => i.Equipo).WithMany().HasForeignKey(i => i.EquipoId).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(i => new { i.TorneoId, i.EquipoId }).IsUnique();
    }
}
