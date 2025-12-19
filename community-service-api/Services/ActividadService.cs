using community_service_api.DbContext;
using community_service_api.Helpers;
using community_service_api.Models.DBTableEntities;
using community_service_api.Models.Dtos;
using community_service_api.Repositories;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

namespace community_service_api.Services;

public interface IActividadService
{
    Task<IEnumerable<Actividad>> GetAllAsync();
    Task<ActividadDto?> GetByIdAsync(int id);
    Task<ActividadDto> CreateAsync(ActividadCreateDto dto);
    Task<bool> UpdateAsync(int id, ActividadUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<InscripcionActividadResponseDto> InscribirUsuarioAsync(InscribirUsuarioActividadRequestDto dto);
    Task<DesinscripcionActividadResponseDto> DesinscribirUsuarioAsync(InscribirUsuarioActividadRequestDto dto);
    Task<IEnumerable<ActividadDetalleDto>> GetVigentesDetalleAsync(int idUsuario);
    Task<MisHorasDto> GetMisHorasAsync(int idUsuario);
}

public class ActividadService : IActividadService
{
    private readonly IRepository<Actividad> _repository;
    private readonly NewApplicationDbContext _db;

    public ActividadService(IRepository<Actividad> repository, NewApplicationDbContext db)
    {
        _repository = repository;
        _db = db;
    }

    public async Task<IEnumerable<Actividad>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<ActividadDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<ActividadDto> CreateAsync(ActividadCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, ActividadUpdateDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
        {
            return false;
        }

        entity.UpdateFromDto(dto);
        await _repository.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<InscripcionActividadResponseDto> InscribirUsuarioAsync(InscribirUsuarioActividadRequestDto dto)
    {
        // Validaciones de entrada básicas (ApiController también valida, pero mantenemos defensa en profundidad)
        if (dto.IdUsuario <= 0 || dto.IdOrganizacion <= 0 || dto.IdActividad <= 0 || dto.IdHorarioActividad <= 0)
        {
            throw new ArgumentException("IdUsuario, IdOrganizacion, IdActividad e IdHorarioActividad deben ser mayores a 0.");
        }

        // Usamos transacción para que: validar -> restar cupo -> crear participante sea atómico.
        await using var tx = await _db.Database.BeginTransactionAsync();

        // 1) Validar rol voluntario (IdRol = 4) en la organización
        var esVoluntario = await _db.RolUsuarioOrganizacion
            .AsNoTracking()
            .AnyAsync(r =>
                r.IdOrganizacion == dto.IdOrganizacion &&
                r.IdUsuarioAsignado == dto.IdUsuario &&
                r.IdRol == 4 &&
                r.Estado == "A" &&
                r.EsActivo == "A");

        if (!esVoluntario)
        {
            throw new UnauthorizedAccessException("El usuario no tiene el rol Voluntario (IdRol=4) activo en la organización.");
        }

        // 2) Evitar doble inscripción y reusar registro si el usuario ya se había inscrito y luego se desinscribió
        var participanteExistente = await _db.ParticipanteActividad
            .Where(p =>
                p.IdOrganizacion == dto.IdOrganizacion &&
                p.IdActividad == dto.IdActividad &&
                p.IdHorarioActividad == dto.IdHorarioActividad &&
                p.IdUsuarioVoluntario == dto.IdUsuario &&
                p.Estado == "A")
            .OrderByDescending(p => p.FechaInscripcion)
            .FirstOrDefaultAsync();

        if (participanteExistente is not null && participanteExistente.FechaHasta == null)
        {
            // Ya está activo
            throw new InvalidOperationException("El usuario ya está inscrito en la actividad.");
        }

        // 3) La actividad debe existir y pertenecer a la organización
        var actividadExiste = await _db.Actividad
            .AsNoTracking()
            .AnyAsync(a => a.IdActividad == dto.IdActividad && a.IdOrganizacion == dto.IdOrganizacion);

        if (!actividadExiste)
        {
            throw new KeyNotFoundException("No existe la actividad para la organización indicada.");
        }

        // 4) Validar que el horario exista y corresponda a la actividad/organización (FK/NOT NULL en PARTICIPANTE_ACTIVIDAD)
        var horarioValido = await _db.HorarioActividad
            .AsNoTracking()
            .AnyAsync(h =>
                h.IdHorarioActividad == dto.IdHorarioActividad &&
                h.IdOrganizacion == dto.IdOrganizacion &&
                h.IdActividad == dto.IdActividad &&
                (h.Situacion == "I" ||
                h.Situacion == "P") &&
                h.Estado == "A" &&
                h.FechaHasta == null);

        if (!horarioValido)
        {
            throw new InvalidOperationException("El IdHorarioActividad no es válido para la actividad/organización indicada o no está activo.");
        }

        // 5) Restar cupo de forma segura (evita concurrencia que deje cupos negativos)
        var affected = await _db.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ACTIVIDAD
            SET CUPOS = CUPOS - 1
            WHERE ID_ACTIVIDAD = {dto.IdActividad}
              AND ID_ORGANIZACION = {dto.IdOrganizacion}
              AND CUPOS > 0");

        if (affected == 0)
        {
            // Puede ser: sin cupos o actividad inexistente (pero ya validamos existencia arriba)
            throw new InvalidOperationException("No hay cupos disponibles para la actividad.");
        }

        // 6) Crear registro o reactivar registro existente (si estaba desinscrito)
        var ahora = DateTime.UtcNow;

        ParticipanteActividad participante;
        if (participanteExistente is not null)
        {
            // Reactivar
            participanteExistente.FechaInscripcion = ahora;
            participanteExistente.FechaRetiro = null;
            participanteExistente.Situacion = "I"; // Inicial / reinscrito
            participanteExistente.FechaDesde = ahora;
            participanteExistente.FechaHasta = null;
            // Estado se mantiene "A"

            await _db.SaveChangesAsync();
            participante = participanteExistente;
        }
        else
        {
            // Nuevo
            participante = new ParticipanteActividad
            {
                IdOrganizacion = dto.IdOrganizacion,
                IdActividad = dto.IdActividad,
                IdHorarioActividad = dto.IdHorarioActividad,
                IdUsuarioVoluntario = dto.IdUsuario,
                FechaInscripcion = ahora,
                FechaRetiro = null,
                Situacion = "I", // Inicial
                FechaDesde = ahora,
                FechaHasta = null,
                Estado = "A"
            };

            _db.ParticipanteActividad.Add(participante);
            await _db.SaveChangesAsync();
        }

        await tx.CommitAsync();

        // Cupos restantes: leemos valor actual (ya persistido).
        var cuposRestantes = await _db.Actividad
            .AsNoTracking()
            .Where(a => a.IdActividad == dto.IdActividad && a.IdOrganizacion == dto.IdOrganizacion)
            .Select(a => a.Cupos)
            .FirstAsync();

        return new InscripcionActividadResponseDto
        {
            IdParticipanteActividad = participante.IdParticipanteActividad,
            IdUsuario = dto.IdUsuario,
            IdOrganizacion = dto.IdOrganizacion,
            IdActividad = dto.IdActividad,
            IdHorarioActividad = dto.IdHorarioActividad,
            FechaInscripcion = participante.FechaInscripcion,
            CuposRestantes = cuposRestantes
        };
    }

    public async Task<DesinscripcionActividadResponseDto> DesinscribirUsuarioAsync(InscribirUsuarioActividadRequestDto dto)
    {
        if (dto.IdUsuario <= 0 || dto.IdOrganizacion <= 0 || dto.IdActividad <= 0 || dto.IdHorarioActividad <= 0)
        {
            throw new ArgumentException("IdUsuario, IdOrganizacion, IdActividad e IdHorarioActividad deben ser mayores a 0.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync();

        // Opcional (consistente con inscripción): validar rol voluntario (IdRol=4)
        var esVoluntario = await _db.RolUsuarioOrganizacion
            .AsNoTracking()
            .AnyAsync(r =>
                r.IdOrganizacion == dto.IdOrganizacion &&
                r.IdUsuarioAsignado == dto.IdUsuario &&
                r.IdRol == 4 &&
                r.Estado == "A" &&
                r.EsActivo == "A");

        if (!esVoluntario)
        {
            throw new UnauthorizedAccessException("El usuario no tiene el rol Voluntario (IdRol=4) activo en la organización.");
        }

        // Buscar inscripción activa exacta para este horario
        var participante = await _db.ParticipanteActividad
            .SingleOrDefaultAsync(p =>
                p.IdOrganizacion == dto.IdOrganizacion &&
                p.IdActividad == dto.IdActividad &&
                p.IdHorarioActividad == dto.IdHorarioActividad &&
                p.IdUsuarioVoluntario == dto.IdUsuario &&
                p.Estado == "A" &&
                p.FechaHasta == null);

        if (participante is null)
        {
            throw new InvalidOperationException("El usuario no está inscrito (o ya fue desinscrito) en este evento/horario.");
        }

        var ahora = DateTime.UtcNow;
        participante.FechaRetiro = ahora;
        participante.Situacion = "R"; // Retirado
        participante.FechaHasta = ahora;

        await _db.SaveChangesAsync();

        // Sumar cupo a la actividad
        await _db.Database.ExecuteSqlInterpolatedAsync($@"
UPDATE ACTIVIDAD
SET CUPOS = CUPOS + 1
WHERE ID_ACTIVIDAD = {dto.IdActividad}
  AND ID_ORGANIZACION = {dto.IdOrganizacion}");

        await tx.CommitAsync();

        var cuposRestantes = await _db.Actividad
            .AsNoTracking()
            .Where(a => a.IdActividad == dto.IdActividad && a.IdOrganizacion == dto.IdOrganizacion)
            .Select(a => a.Cupos)
            .FirstOrDefaultAsync();

        return new DesinscripcionActividadResponseDto
        {
            IdUsuario = dto.IdUsuario,
            IdOrganizacion = dto.IdOrganizacion,
            IdActividad = dto.IdActividad,
            IdHorarioActividad = dto.IdHorarioActividad,
            FechaRetiro = participante.FechaRetiro ?? ahora,
            CuposRestantes = cuposRestantes
        };
    }

    public async Task<IEnumerable<ActividadDetalleDto>> GetVigentesDetalleAsync(int idUsuario)
    {
        // "Vigentes / no vencidas" por horario: debe existir al menos un HorarioActividad cuyo HoraFin sea >= ahora.
        if (idUsuario <= 0)
        {
            throw new ArgumentException("idUsuario debe ser mayor a 0.");
        }

        var ahora = DateTime.UtcNow;

        // JOIN explícito para no depender de navigations (no hay lazy-loading).
        // Filtra: organizaciones donde el usuario es voluntario (IdRol=4) y actividades con al menos un horario activo y no vencido.
        var flat = await (
                from a in _db.Actividad.AsNoTracking()
                join ruo in _db.RolUsuarioOrganizacion.AsNoTracking()
                    on a.IdOrganizacion equals ruo.IdOrganizacion
                join o in _db.Organizacion.AsNoTracking()
                    on a.IdOrganizacion equals o.IdOrganizacion
                join u in _db.Usuario.AsNoTracking()
                    on a.IdUsuarioCreador equals u.IdUsuario
                join c in _db.CategoriaActividad.AsNoTracking()
                    on a.IdCategoria equals c.IdCategoriaActividad
                join ub in _db.Ubicacion.AsNoTracking()
                    on a.IdUbicacion equals ub.IdUbicacion
                join h in _db.HorarioActividad.AsNoTracking()
                    on new { a.IdOrganizacion, a.IdActividad }
                    equals new { h.IdOrganizacion, h.IdActividad }
                join p in _db.ParticipanteActividad.AsNoTracking()
                        .Where(p =>
                            p.IdUsuarioVoluntario == idUsuario &&
                            p.Estado == "A" &&
                            p.FechaHasta == null)
                    on new { h.IdOrganizacion, h.IdActividad, h.IdHorarioActividad }
                    equals new { p.IdOrganizacion, p.IdActividad, p.IdHorarioActividad } into pp
                from p in pp.DefaultIfEmpty()
                where ruo.IdUsuarioAsignado == idUsuario
                      && ruo.IdRol == 4
                      && ruo.Estado == "A"
                      && ruo.EsActivo == "A"
                      && (a.Situacion == "I" || a.Situacion == "P")
                      && a.Estado == "A"
                      && a.FechaHasta == null
                      // Horario activo + no vencido
                      && h.Estado == "A"
                      && h.FechaHasta == null
                      && (h.Situacion == "I" || h.Situacion == "P")
                      && h.HoraFin >= ahora
                select new
                {
                    // Actividad
                    a.IdActividad,
                    a.IdOrganizacion,
                    a.IdUsuarioCreador,
                    a.IdCategoria,
                    a.IdUbicacion,
                    a.Nombre,
                    a.Descripcion,
                    a.FechaInicio,
                    a.FechaFin,
                    a.Horas,
                    a.Cupos,
                    a.Situacion,
                    a.Estado,
                    // Organizacion
                    OrganizacionNombre = o.Nombre,
                    // Usuario creador
                    UsuarioCreadorUsername = u.Username,
                    // Categoria
                    CategoriaNombre = c.Nombre,
                    // Ubicacion
                    UbicacionPais = ub.IdPais,
                    UbicacionProvincia = ub.IdProvincia,
                    UbicacionCanton = ub.IdCanton,
                    UbicacionDistrito = ub.IdDistrito,
                    UbicacionDireccion = ub.Direccion,
                    UbicacionCodigoPostal = ub.CodigoPostal,
                    UbicacionLatitud = ub.Latitud,
                    UbicacionLongitud = ub.Longitud,
                    UbicacionEstado = ub.Estado,
                    // Horario
                    h.IdHorarioActividad,
                    h.Fecha,
                    h.HoraInicio,
                    h.HoraFin,
                    HorarioDescripcion = h.Descripcion,
                    HorarioSituacion = h.Situacion,
                    HorarioEstado = h.Estado,
                    // Usuario inscrito en este horario
                    UsuarioInscrito = p != null
                }
            )
            .ToListAsync();

        var result = flat
            .GroupBy(x => new
            {
                x.IdOrganizacion,
                x.IdActividad,
                x.IdUsuarioCreador,
                x.IdCategoria,
                x.IdUbicacion,
                x.Nombre,
                x.Descripcion,
                x.FechaInicio,
                x.FechaFin,
                x.Horas,
                x.Cupos,
                x.Situacion,
                x.Estado,
                x.OrganizacionNombre,
                x.UsuarioCreadorUsername,
                x.CategoriaNombre,
                x.UbicacionPais,
                x.UbicacionProvincia,
                x.UbicacionCanton,
                x.UbicacionDistrito,
                x.UbicacionDireccion,
                x.UbicacionCodigoPostal,
                x.UbicacionLatitud,
                x.UbicacionLongitud,
                x.UbicacionEstado
            })
            .Select(g => new ActividadDetalleDto
            {
                IdActividad = g.Key.IdActividad,
                IdOrganizacion = g.Key.IdOrganizacion,
                IdUsuarioCreador = g.Key.IdUsuarioCreador,
                IdCategoria = g.Key.IdCategoria,
                IdUbicacion = g.Key.IdUbicacion,
                Nombre = g.Key.Nombre,
                Descripcion = g.Key.Descripcion,
                FechaInicio = g.Key.FechaInicio,
                FechaFin = g.Key.FechaFin,
                Horas = g.Key.Horas,
                Cupos = g.Key.Cupos,
                Situacion = g.Key.Situacion[0],
                Estado = g.Key.Estado[0],
                Organizacion = new OrganizacionBasicaDto
                {
                    IdOrganizacion = g.Key.IdOrganizacion,
                    Nombre = g.Key.OrganizacionNombre
                },
                UsuarioCreador = new UsuarioBasicoDto
                {
                    IdUsuario = g.Key.IdUsuarioCreador,
                    Username = g.Key.UsuarioCreadorUsername
                },
                Categoria = new CategoriaActividadBasicaDto
                {
                    IdCategoriaActividad = g.Key.IdCategoria,
                    Nombre = g.Key.CategoriaNombre
                },
                Ubicacion = new UbicacionBasicaDto
                {
                    IdUbicacion = g.Key.IdUbicacion,
                    IdPais = g.Key.UbicacionPais,
                    IdProvincia = g.Key.UbicacionProvincia,
                    IdCanton = g.Key.UbicacionCanton,
                    IdDistrito = g.Key.UbicacionDistrito,
                    Direccion = g.Key.UbicacionDireccion,
                    CodigoPostal = g.Key.UbicacionCodigoPostal,
                    Latitud = g.Key.UbicacionLatitud,
                    Longitud = g.Key.UbicacionLongitud,
                    Estado = g.Key.UbicacionEstado[0]
                },
                Horarios = g
                    .GroupBy(x => x.IdHorarioActividad)
                    .Select(hg => new HorarioActividadBasicoDto
                    {
                        IdHorarioActividad = hg.Key,
                        IdOrganizacion = g.Key.IdOrganizacion,
                        IdActividad = g.Key.IdActividad,
                        Fecha = hg.Max(x => x.Fecha),
                        HoraInicio = hg.Max(x => x.HoraInicio),
                        HoraFin = hg.Max(x => x.HoraFin),
                        Descripcion = hg.Select(x => x.HorarioDescripcion).FirstOrDefault(),
                        Situacion = hg.Select(x => x.HorarioSituacion[0]).First(),
                        Estado = hg.Select(x => x.HorarioEstado[0]).First(),
                        UsuarioInscrito = hg.Any(x => x.UsuarioInscrito)
                    })
                    .OrderBy(x => x.Fecha)
                    .ThenBy(x => x.HoraInicio)
                    .ToList()
            })
            .ToList();

        return result;
    }

    public async Task<MisHorasDto> GetMisHorasAsync(int idUsuario)
    {
        if (idUsuario <= 0)
        {
            throw new ArgumentException("idUsuario debe ser mayor a 0.");
        }

        var ahora = DateTime.UtcNow;

        // Tomamos participaciones "válidas" para acumulación:
        // - ParticipanteActividad activo (Estado='A') y vigente (FechaHasta null)
        // - El usuario debió estar inscrito antes de que terminara el horario (FechaInscripcion <= HoraFin)
        // - Horario activo y no borrado lógico
        // - Horario ya finalizó (HoraFin <= ahora) para contar horas ya realizadas

        // Nota: usamos JOIN explícito para no depender de navigations (FK compuesta hacia HorarioActividad).
        //        p.FechaHasta == null(inscripción vigente / activa)
        //p.FechaInscripcion <= h.HoraFin(evita contar horas si el usuario se inscribe / re - activa después de que el horario ya terminó)
        var rows = await (
                from p in _db.ParticipanteActividad.AsNoTracking()
                join h in _db.HorarioActividad.AsNoTracking()
                    on new { p.IdHorarioActividad, p.IdOrganizacion, p.IdActividad }
                    equals new { h.IdHorarioActividad, h.IdOrganizacion, h.IdActividad }
                join a in _db.Actividad.AsNoTracking()
                    on new { p.IdActividad, p.IdOrganizacion }
                    equals new { a.IdActividad, a.IdOrganizacion }
                where p.IdUsuarioVoluntario == idUsuario
                      && p.Estado == "A"
                      && p.FechaHasta == null
                      && p.FechaInscripcion <= h.HoraFin
                      && h.Estado == "A"
                      && h.FechaHasta == null
                      && h.HoraFin <= ahora
                select new
                {
                    p.IdOrganizacion,
                    p.IdActividad,
                    ActividadNombre = a.Nombre,
                    Fecha = h.Fecha,
                    HoraInicio = h.HoraInicio,
                    HoraFin = h.HoraFin
                }
            )
            .ToListAsync();

        decimal DuracionHoras(DateTime inicio, DateTime fin)
        {
            var hours = (fin - inicio).TotalHours;
            if (hours < 0) hours = 0;
            // 2 decimales para soportar medias horas, etc.
            return Math.Round((decimal)hours, 2);
        }

        var desglose = rows
            .GroupBy(r => new { r.IdOrganizacion, r.IdActividad, r.ActividadNombre })
            .Select(g => new MisHorasDetalleDto
            {
                IdOrganizacion = g.Key.IdOrganizacion,
                IdActividad = g.Key.IdActividad,
                Actividad = g.Key.ActividadNombre,
                Fecha = g.Max(x => x.Fecha),
                Horas = g.Sum(x => DuracionHoras(x.HoraInicio, x.HoraFin))
            })
            .OrderByDescending(x => x.Fecha)
            .ToList();

        return new MisHorasDto
        {
            HorasTotales = desglose.Sum(x => x.Horas),
            Actividades = desglose.Count,
            UltimaParticipacion = desglose.Count == 0 ? null : desglose.Max(x => x.Fecha),
            Desglose = desglose
        };
    }
}
