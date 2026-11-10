using ArenaHub.Domain.Common;

namespace ArenaHub.Domain.Torneos;

public static class TorneoErrores
{
    public static readonly Error NombreRequerido =
        Error.Validation("Torneo.NombreRequerido", "El nombre del torneo es obligatorio.");

    public static readonly Error FinalSinResolver =
        Error.Conflict("Torneo.FinalSinResolver", "El torneo se finaliza solo al cargar el ganador de la final.");

    public static Error NoEncontrado(int id) =>
        Error.NotFound("Torneo.NoEncontrado", $"No existe el torneo {id}.");

    public static Error CupoInvalido(int cupo) =>
        Error.Conflict("Torneo.CupoInvalido", $"El cupo debe ser de 4, 8 o 16 equipos (se pidió {cupo}).");

    public static Error TransicionInvalida(EstadoTorneo desde, EstadoTorneo hacia) =>
        Error.Conflict("Torneo.TransicionInvalida", $"No se puede pasar de {desde} a {hacia}.");

    public static Error CupoIncompleto(int inscriptos, int cupo) =>
        Error.Conflict("Torneo.CupoIncompleto", $"Hay {inscriptos} de {cupo} equipos inscriptos.");
}
