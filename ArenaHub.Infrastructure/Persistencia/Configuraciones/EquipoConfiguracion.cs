using ArenaHub.Domain.Equipos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArenaHub.Infrastructure.Persistencia.Configuraciones;

internal sealed class EquipoConfiguracion : IEntityTypeConfiguration<Equipo>
{
    public void Configure(EntityTypeBuilder<Equipo> builder)
    {
        builder.Property(e => e.Nombre).HasMaxLength(100);

        builder.HasMany(e => e.Jugadores)
            .WithOne()
            .HasForeignKey(j => j.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
