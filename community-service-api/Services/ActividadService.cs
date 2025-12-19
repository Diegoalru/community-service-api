using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.DbContext;
using community_service_api.Helpers;
using community_service_api.Models.DBTableEntities;
using community_service_api.Models.Dtos;
using community_service_api.Repositories;
using Microsoft.EntityFrameworkCore;

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

        return await _db.Actividad
            .AsNoTracking()
            .Where(a =>
                // Actividad en estado permitido
                (a.Situacion == "I" || a.Situacion == "P") &&
                // Usuario asociado a la organización como Voluntario (IdRol=4)
                _db.RolUsuarioOrganizacion.Any(r =>
                    r.IdOrganizacion == a.IdOrganizacion &&
                    r.IdUsuarioAsignado == idUsuario &&
                    r.IdRol == 4 &&
                    r.Estado == "A" &&
                    r.EsActivo == "A") &&
                // Debe existir al menos un horario activo (y no vencido)
                a.HorarioActividad.Any(h =>
                    h.Estado == "A" &&
                    h.FechaHasta == null &&
                    (h.Situacion == "I" || h.Situacion == "P") &&
                    h.HoraFin >= ahora))
            .Select(a => new ActividadDetalleDto
            {
                IdActividad = a.IdActividad,
                IdOrganizacion = a.IdOrganizacion,
                IdUsuarioCreador = a.IdUsuarioCreador,
                IdCategoria = a.IdCategoria,
                IdUbicacion = a.IdUbicacion,
                Nombre = a.Nombre,
                Descripcion = a.Descripcion,
                FechaInicio = a.FechaInicio,
                FechaFin = a.FechaFin,
                Horas = a.Horas,
                Cupos = a.Cupos,
                Situacion = a.Situacion[0],
                Estado = a.Estado[0],
                Organizacion = new OrganizacionBasicaDto
                {
                    IdOrganizacion = a.IdOrganizacionNavigation.IdOrganizacion,
                    Nombre = a.IdOrganizacionNavigation.Nombre
                },
                UsuarioCreador = new UsuarioBasicoDto
                {
                    IdUsuario = a.IdUsuarioCreadorNavigation.IdUsuario,
                    Username = a.IdUsuarioCreadorNavigation.Username
                },
                Categoria = new CategoriaActividadBasicaDto
                {
                    IdCategoriaActividad = a.IdCategoriaNavigation.IdCategoriaActividad,
                    Nombre = a.IdCategoriaNavigation.Nombre
                },
                Ubicacion = new UbicacionBasicaDto
                {
                    IdUbicacion = a.IdUbicacionNavigation.IdUbicacion,
                    IdPais = a.IdUbicacionNavigation.IdPais,
                    IdProvincia = a.IdUbicacionNavigation.IdProvincia,
                    IdCanton = a.IdUbicacionNavigation.IdCanton,
                    IdDistrito = a.IdUbicacionNavigation.IdDistrito,
                    Direccion = a.IdUbicacionNavigation.Direccion,
                    CodigoPostal = a.IdUbicacionNavigation.CodigoPostal,
                    Latitud = a.IdUbicacionNavigation.Latitud,
                    Longitud = a.IdUbicacionNavigation.Longitud,
                    Estado = a.IdUbicacionNavigation.Estado[0]
                },
                Horarios = a.HorarioActividad
                    .Where(h =>
                        h.Estado == "A" &&
                        h.FechaHasta == null &&
                        (h.Situacion == "I" || h.Situacion == "P"))
                    .OrderBy(h => h.Fecha)
                    .ThenBy(h => h.HoraInicio)
                    .Select(h => new HorarioActividadBasicoDto
                    {
                        IdHorarioActividad = h.IdHorarioActividad,
                        IdOrganizacion = h.IdOrganizacion,
                        IdActividad = h.IdActividad,
                        Fecha = h.Fecha,
                        HoraInicio = h.HoraInicio,
                        HoraFin = h.HoraFin,
                        Descripcion = h.Descripcion,
                        Situacion = h.Situacion[0],
                        Estado = h.Estado[0]
                    })
                    .ToList(),
                UsuarioInscrito = a.ParticipanteActividad.Any(p =>
                    p.IdUsuarioVoluntario == idUsuario &&
                    p.Estado == "A" &&
                    p.FechaHasta == null)
            })
            .ToListAsync();
    }
}
