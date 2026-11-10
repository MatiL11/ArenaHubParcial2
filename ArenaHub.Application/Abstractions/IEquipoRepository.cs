using ArenaHub.Domain.Equipos;

namespace ArenaHub.Application.Abstractions;

public interface IEquipoRepository
{
    Task<Equipo?> ObtenerAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<Equipo>> ListarPorRatingAsync(CancellationToken ct);
    void Agregar(Equipo equipo);
}
