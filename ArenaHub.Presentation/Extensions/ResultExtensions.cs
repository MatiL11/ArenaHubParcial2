using ArenaHub.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace ArenaHub.Presentation.Extensions;

public static class ResultExtensions
{
    // Único punto donde un Error del dominio se traduce a HTTP.
    public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> onSuccess)
    {
        if (result.IsSuccess) return onSuccess(result.Value);

        var error = result.Error!;
        var (status, titulo) = error.Type switch
        {
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Solicitud inválida"),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Recurso no encontrado"),
            _ => (StatusCodes.Status409Conflict, "Conflicto con una regla de negocio")
        };

        var problema = new ProblemDetails { Status = status, Title = titulo, Detail = error.Message };
        problema.Extensions["errorCode"] = error.Code;

        return new ObjectResult(problema) { StatusCode = status, ContentTypes = { "application/problem+json" } };
    }
}
