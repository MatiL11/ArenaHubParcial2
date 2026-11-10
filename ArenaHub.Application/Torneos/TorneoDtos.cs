using ArenaHub.Domain.Torneos;

namespace ArenaHub.Application.Torneos;

public sealed record CrearTorneoRequest(string Nombre, int Cupo);

public sealed record TransicionRequest(EstadoTorneo Destino);

public sealed record TorneoDto(int Id, string Nombre, int Cupo, EstadoTorneo Estado, int Inscriptos, int? CampeonId)
{
    internal static TorneoDto Desde(Torneo torneo) =>
        new(torneo.Id, torneo.Nombre, torneo.Cupo, torneo.Estado, torneo.Inscripciones.Count, torneo.CampeonId);
}
