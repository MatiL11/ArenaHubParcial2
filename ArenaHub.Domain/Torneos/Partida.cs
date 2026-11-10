using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Torneos;

public sealed class Partida
{
    internal Partida(int ronda, int numero, int equipoAId, int equipoBId)
    {
        Ronda = ronda;
        Numero = numero;
        EquipoAId = equipoAId;
        EquipoBId = equipoBId;
    }

    public int Id { get; private set; }
    public int TorneoId { get; private set; }
    public int Ronda { get; private set; }
    public int Numero { get; private set; }
    public int EquipoAId { get; private set; }
    public int EquipoBId { get; private set; }
    public int? GanadorId { get; private set; }

    public bool Resuelta => GanadorId is not null;

    internal Result Resolver(int ganadorId)
    {
        if (Resuelta) return PartidaErrores.YaResuelta(Id);
        if (ganadorId != EquipoAId && ganadorId != EquipoBId) return PartidaErrores.GanadorInvalido(ganadorId);

        GanadorId = ganadorId;
        return Result.Success();
    }
}
