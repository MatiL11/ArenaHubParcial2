using ArenaHub.Domain.Jugadores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArenaHub.Infrastructure.Persistencia.Configuraciones;

internal sealed class JugadorConfiguracion : IEntityTypeConfiguration<Jugador>
{
    public void Configure(EntityTypeBuilder<Jugador> builder)
    {
        builder.Property(j => j.Nickname).HasMaxLength(50);
        builder.HasIndex(j => j.Nickname).IsUnique();
    }
}
