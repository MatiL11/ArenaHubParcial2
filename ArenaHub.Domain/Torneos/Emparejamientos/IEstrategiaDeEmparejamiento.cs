namespace ArenaHub.Domain.Torneos.Emparejamientos;

public sealed record Cruce(int EquipoAId, int EquipoBId);

public interface IEstrategiaDeEmparejamiento
{
    IReadOnlyList<Cruce> Emparejar(IReadOnlyList<int> equipos);
}
