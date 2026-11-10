using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Common;
using ArenaHub.Domain.Equipos;
using ArenaHub.Domain.Jugadores;

namespace ArenaHub.Application.Equipos;

public sealed class EquiposService(IEquipoRepository equipos, IJugadorRepository jugadores, IUnitOfWork unitOfWork)
{
    public async Task<Result<EquipoDto>> CrearAsync(CrearEquipoRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre)) return EquipoErrores.NombreRequerido;

        var equipo = new Equipo(request.Nombre.Trim());
        equipos.Agregar(equipo);
        await unitOfWork.SaveChangesAsync(ct);
        return EquipoDto.Desde(equipo);
    }

    public async Task<Result<EquipoDto>> ObtenerAsync(int id, CancellationToken ct) =>
        await equipos.ObtenerAsync(id, ct) is { } equipo
            ? EquipoDto.Desde(equipo)
            : EquipoErrores.NoEncontrado(id);

    public async Task<Result<EquipoDto>> AgregarJugadorAsync(int equipoId, AgregarJugadorRequest request,
        CancellationToken ct)
    {
        var equipo = await equipos.ObtenerAsync(equipoId, ct);
        if (equipo is null) return EquipoErrores.NoEncontrado(equipoId);

        var jugador = await jugadores.ObtenerAsync(request.JugadorId, ct);
        if (jugador is null) return JugadorErrores.NoEncontrado(request.JugadorId);

        var resultado = equipo.AgregarJugador(jugador);
        if (resultado.IsFailure) return resultado.Error!;

        await unitOfWork.SaveChangesAsync(ct);
        return EquipoDto.Desde(equipo);
    }
}
