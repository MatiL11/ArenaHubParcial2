namespace ArenaHub.Domain.Jugadores;

public sealed class Jugador
{
    public Jugador(string nickname) => Nickname = nickname;

    public int Id { get; private set; }
    public string Nickname { get; private set; }
    public int? EquipoId { get; private set; }

    internal void AsignarEquipo(int equipoId) => EquipoId = equipoId;
}
