using ArenaHub.Application.Abstractions;
using ArenaHub.Domain.Common;
using ArenaHub.Domain.Jugadores;

namespace ArenaHub.Application.Jugadores;

public sealed class JugadoresService(IJugadorRepository jugadores, IUnitOfWork unitOfWork)
{
    public async Task<Result<JugadorDto>> CrearAsync(CrearJugadorRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Nickname)) return JugadorErrores.NicknameRequerido;

        var nickname = request.Nickname.Trim();
        if (await jugadores.ExisteNicknameAsync(nickname, ct)) return JugadorErrores.NicknameDuplicado(nickname);

        var jugador = new Jugador(nickname);
        jugadores.Agregar(jugador);
        await unitOfWork.SaveChangesAsync(ct);
        return JugadorDto.Desde(jugador);
    }

    public async Task<Result<JugadorDto>> ObtenerAsync(int id, CancellationToken ct) =>
        await jugadores.ObtenerAsync(id, ct) is { } jugador
            ? JugadorDto.Desde(jugador)
            : JugadorErrores.NoEncontrado(id);
}
