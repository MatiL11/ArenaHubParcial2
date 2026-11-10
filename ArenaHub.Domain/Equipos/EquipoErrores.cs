using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Equipos;

public static class EquipoErrores
{
    public static readonly Error NombreRequerido =
        Error.Validation("Equipo.NombreRequerido", "El nombre del equipo es obligatorio.");

    public static Error NoEncontrado(int id) =>
        Error.NotFound("Equipo.NoEncontrado", $"No existe el equipo {id}.");

    public static Error Completo(string nombre) =>
        Error.Conflict("Equipo.Completo", $"El equipo '{nombre}' ya tiene {Equipo.MaximoDeJugadores} jugadores.");
}
