using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Jugadores;

public static class JugadorErrores
{
    public static readonly Error NicknameRequerido =
        Error.Validation("Jugador.NicknameRequerido", "El nickname es obligatorio.");

    public static Error NoEncontrado(int id) =>
        Error.NotFound("Jugador.NoEncontrado", $"No existe el jugador {id}.");

    public static Error NicknameDuplicado(string nickname) =>
        Error.Conflict("Jugador.NicknameDuplicado", $"Ya existe un jugador con el nickname '{nickname}'.");

    public static Error YaTieneEquipo(string nickname) =>
        Error.Conflict("Jugador.YaTieneEquipo", $"El jugador '{nickname}' ya pertenece a un equipo.");
}
