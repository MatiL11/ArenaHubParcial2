using ArenaHub.Domain.Torneos;

namespace ArenaHub.Application.Partidas;

public sealed record RegistrarResultadoRequest(int GanadorId);

public sealed record PartidaDto(int Id, int Ronda, int Numero, int EquipoAId, int EquipoBId, int? GanadorId)
{
    internal static PartidaDto Desde(Partida partida) =>
        new(partida.Id, partida.Ronda, partida.Numero, partida.EquipoAId, partida.EquipoBId, partida.GanadorId);
}
