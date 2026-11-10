# ArenaHub

API REST para torneos universitarios de eSports: jugadores, equipos, inscripciones, un bracket de eliminación simple y un ranking por rating Elo.

Está hecha en .NET 10 con Clean Architecture, EF Core sobre SQLite y Swagger.

## Cómo correrlo

Requisito: SDK de .NET 10.

```bash
dotnet run --project ArenaHub.Presentation
```

- Swagger queda en http://localhost:5116/swagger.
- Al arrancar se aplican las migraciones de EF Core y se crea `arenahub.db` en `ArenaHub.Presentation/`. Para empezar de cero alcanza con borrar ese archivo.
- La conexión está en `ArenaHub.Presentation/appsettings.json`, en `ConnectionStrings:ArenaHub`.

## Tests

```bash
dotnet test
```

| Archivo | Qué prueba |
|---|---|
| `EloTests.cs` | Los casos E1, E2 y E3 del enunciado, y el factor K según las series jugadas |
| `EmparejamientoTests.cs` | Siembra de 4, 8 y 16 equipos, y cruce de ganadores entre rondas |
| `FlujoCompletoTests.cs` | Los 7 pasos del flujo contra la API, con SQLite en memoria: cuatro equipos, torneo de cupo 4, bracket 1 vs 4 y 2 vs 3, final, campeón y ranking. También que una partida resuelta devuelva 409 |

Para regenerar la migración se usa la herramienta local (`dotnet tool restore` la instala):

```bash
dotnet ef migrations add <Nombre> -p ArenaHub.Infrastructure -s ArenaHub.Presentation -o Persistencia/Migraciones
```

## Estructura

```
ArenaHub.Domain          Entidades, reglas de negocio, Result/Error, Elo y los tres patrones
ArenaHub.Application     Un servicio por controller, DTOs e interfaces de repositorio
ArenaHub.Infrastructure  DbContext, configuraciones de EF Core, repositorios y migraciones
ArenaHub.Presentation    Controllers, mapeo de Error a HTTP y composición (Program.cs)
ArenaHub.Tests           xUnit
```

Las dependencias apuntan hacia adentro:

- Application depende de Domain.
- Infrastructure implementa las interfaces de Application.
- Presentation conoce Infrastructure solo en `Program.cs`, para registrarla y aplicar las migraciones al arrancar.

Los controllers no tienen lógica. Cada acción llama a su servicio y convierte el resultado con `ToActionResult`.

## Result Pattern y errores

`Result` y `Result<T>` están en `ArenaHub.Domain/Common`. Ningún caso de uso lanza excepciones: el dominio y los servicios devuelven un `Error` con `Code`, `Message` y `Type`.

La traducción a HTTP está en un único lugar, `ArenaHub.Presentation/Extensions/ResultExtensions.cs`. Responde con `ProblemDetails` y el código del error en `extensions.errorCode`.

| ErrorType | HTTP | Ejemplo |
|---|---|---|
| Validation | 400 | `Jugador.NicknameRequerido` |
| NotFound | 404 | `Torneo.NoEncontrado` |
| Conflict | 409 | `Torneo.TransicionInvalida`, `Inscripcion.CupoCompleto`, `Partida.YaResuelta` |

## Rating Elo

Todos los equipos empiezan con 1000 puntos. Al resolverse una partida se actualizan los dos equipos, cada uno con los ratings previos a la partida:

```
E  = 1 / (1 + 10^((R_rival - R) / 400))     resultado esperado
R' = R + K · (S - E)                         S = 1 si ganó, 0 si perdió
```

- K vale 40 si el equipo jugó menos de 10 series y 20 desde la décima. Así un equipo nuevo se acomoda rápido y uno con historial cambia menos.
- El resultado se redondea con `MidpointRounding.AwayFromZero`.
- Ganarle a un rival más fuerte da más puntos, porque E es más bajo.

| Caso | A (rating, series) | B (rating, series) | Gana | A después | B después |
|---|---|---|---|---|---|
| E1 | 1000, 0 | 1000, 0 | A | 1020 | 980 |
| E2 | 1500, 15 | 1700, 15 | A | 1515 | 1685 |
| E3 | 1200, 12 | 1000, 3 | B | 1185 | 1030 |

En E3, A pierde 15 puntos con K=20 y B gana 30 con K=40. Por eso los puntos no son simétricos.

El cálculo está en `ArenaHub.Domain/Equipos/Elo.cs`.

## Patrones de diseño

### State: estados del torneo

**Problema:** el torneo solo puede avanzar Borrador → InscripcionAbierta → EnCurso → Finalizado, y cada paso tiene sus condiciones. Por ejemplo, para pasar a EnCurso el cupo tiene que estar completo, y en ese momento se genera la primera ronda.

**Archivos:**
- `Domain/Torneos/Estados/`: `EstadoDelTorneo` y un estado por clase.
- `Torneo.PasarA` delega en el estado actual.

**Sin el patrón:** `Torneo` tendría un `switch` sobre el estado de origen y otro sobre el destino. Cada regla nueva lo agrandaría. Con State, cada clase habilita solo su transición y cualquier otra cae en el 409 por defecto de la base.

### Specification: reglas de inscripción (RN-06)

**Problema:** inscribir un equipo exige cuatro condiciones independientes:
- el torneo tiene la inscripción abierta;
- el equipo tiene entre 3 y 5 jugadores;
- el equipo no está inscripto;
- queda cupo.

**Archivos:**
- `Domain/Common/Especificacion.cs`: la base y la combinación con `Y`.
- `Domain/Torneos/Inscripciones/ReglasDeInscripcion.cs`: una clase por regla.

**Sin el patrón:** serían cuatro `if` dentro de `Torneo.Inscribir`, mezclados con el alta. Así, cada regla se lee, se prueba y se reordena por separado, y sumar una nueva es agregar una clase a la cadena `Todas`.

### Strategy: emparejamientos del bracket

**Problema:** la primera ronda se arma por siembra (1 vs 8, 4 vs 5, 2 vs 7, 3 vs 6) y las siguientes cruzan ganadores en orden (ganador de la partida 1 contra el de la 2). Son dos algoritmos distintos para la misma operación.

**Archivos:**
- `Domain/Torneos/Emparejamientos/`: la interfaz `IEstrategiaDeEmparejamiento`, `EmparejamientoPorSiembra` y `EmparejamientoDeGanadores`.
- `Torneo.GenerarRonda` recibe la estrategia que corresponde.

**Sin el patrón:** `GenerarRonda` tendría un `if (ronda == 1)` con los dos algoritmos adentro. Con Strategy cada algoritmo se prueba solo (`EmparejamientoTests`), y la siembra recursiva sirve igual para 4, 8 y 16 equipos.

## Decisiones

- **Siembra:** se ordena por rating descendente. Si dos equipos empatan, va primero el que se inscribió antes. Cuando todos tienen 1000, el orden de inscripción define el cuadro.
- **Cupo inválido:** un cupo distinto de 4, 8 o 16 devuelve 409 (`Torneo.CupoInvalido`), porque es una regla de negocio (RN-05). Un campo vacío devuelve 400.
- **Fin del torneo:** el torneo no se puede pasar a Finalizado a mano. Queda finalizado solo cuando se carga el ganador de la final (RN-11), y si no, devuelve `Torneo.FinalSinResolver`.
- **Empate en el ranking:** se ordena por nombre.
- **Bonus implementados:** cupo de 16 equipos, test de integración del flujo completo y migraciones de EF Core en lugar de `EnsureCreated()`.
