using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Torneos;

public static class PartidaErrores
{
    public static Error NoEncontrada(int id) =>
        Error.NotFound("Partida.NoEncontrada", $"No existe la partida {id}.");

    public static Error YaResuelta(int id) =>
        Error.Conflict("Partida.YaResuelta", $"La partida {id} ya tiene ganador y no se puede modificar.");

    public static Error GanadorInvalido(int equipoId) =>
        Error.Conflict("Partida.GanadorInvalido", $"El equipo {equipoId} no juega esta partida.");
}
