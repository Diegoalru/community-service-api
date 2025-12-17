using community_service_api.Helpers;
using community_service_api.Models.DBTableEntities;
using community_service_api.Models.Dtos;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IAsistenciaActividadService
{
    Task<AsistenciaActividadDto> RegistrarAsync(AsistenciaActividadCreateDto dto);
    Task<IEnumerable<AsistenciaActividadDto>> GetByActividadAsync(int actividadId);
    Task<IEnumerable<AsistenciaActividadDto>> GetByUsuarioAsync(int usuarioId);
    Task<decimal> GetTotalHorasAsync(int usuarioId, int? actividadId);
}

public class AsistenciaActividadService(IRepository<AsistenciaActividad> repository) : IAsistenciaActividadService
{
    public async Task<AsistenciaActividadDto> RegistrarAsync(AsistenciaActividadCreateDto dto)
    {
        if (dto.Horas <= 0 || dto.Horas > 24)
            throw new Exception("Las horas deben ser mayores a 0 y menores o iguales a 24.");

        var fecha = dto.Fecha.Date;

        var all = await repository.GetAllAsync();

        var existente = all.FirstOrDefault(a =>
            a.ActividadId == dto.ActividadId &&
            a.UsuarioId == dto.UsuarioId &&
            a.Fecha.Date == fecha);

        if (existente == null)
        {
            var entity = new AsistenciaActividad
            {
                ActividadId = dto.ActividadId,
                UsuarioId = dto.UsuarioId,
                Fecha = fecha,
                Horas = dto.Horas,
                Observacion = dto.Observacion,
                CreadoEn = DateTime.Now
            };

            await repository.AddAsync(entity);
            return entity.ToDto();
        }

        existente.Horas = dto.Horas;
        existente.Observacion = dto.Observacion;
        existente.ActualizadoEn = DateTime.Now;

        await repository.UpdateAsync(existente);
        return existente.ToDto();
    }

    public async Task<IEnumerable<AsistenciaActividadDto>> GetByActividadAsync(int actividadId)
    {
        var all = await repository.GetAllAsync();
        return all
            .Where(a => a.ActividadId == actividadId)
            .OrderByDescending(a => a.Fecha)
            .Select(a => a.ToDto());
    }

    public async Task<IEnumerable<AsistenciaActividadDto>> GetByUsuarioAsync(int usuarioId)
    {
        var all = await repository.GetAllAsync();
        return all
            .Where(a => a.UsuarioId == usuarioId)
            .OrderByDescending(a => a.Fecha)
            .Select(a => a.ToDto());
    }

    public async Task<decimal> GetTotalHorasAsync(int usuarioId, int? actividadId)
    {
        var all = await repository.GetAllAsync();

        var q = all.Where(a => a.UsuarioId == usuarioId);

        if (actividadId.HasValue)
            q = q.Where(a => a.ActividadId == actividadId.Value);

        return q.Sum(a => a.Horas);
    }
}