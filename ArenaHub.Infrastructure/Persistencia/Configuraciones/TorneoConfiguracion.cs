using ArenaHub.Domain.Equipos;
using ArenaHub.Domain.Torneos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArenaHub.Infrastructure.Persistencia.Configuraciones;

internal sealed class TorneoConfiguracion : IEntityTypeConfiguration<Torneo>
{
    public void Configure(EntityTypeBuilder<Torneo> builder)
    {
        builder.Property(t => t.Nombre).HasMaxLength(100);
        builder.Property(t => t.Estado).HasConversion<string>().HasMaxLength(20);

        builder.HasOne<Equipo>().WithMany().HasForeignKey(t => t.CampeonId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(t => t.Inscripciones).WithOne().HasForeignKey(i => i.TorneoId);
        builder.HasMany(t => t.Partidas).WithOne().HasForeignKey(p => p.TorneoId);
    }
}
