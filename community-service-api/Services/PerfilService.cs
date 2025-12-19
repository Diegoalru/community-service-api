using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.DbContext;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace community_service_api.Services;

public interface IPerfilService
{
    Task<IEnumerable<PerfilDto>> GetAllAsync();
    Task<PerfilDto?> GetByIdAsync(int id);
    Task<PerfilDetalleDto?> GetDetalleByIdAsync(int idPerfil);
    Task<PerfilDetalleDto?> GetDetalleByUsuarioIdAsync(int idUsuario);
    Task<PerfilDto> CreateAsync(PerfilCreateDto dto);
    Task<bool> UpdateAsync(int id, PerfilUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class PerfilService : IPerfilService
{
    private readonly IRepository<Perfil> _repository;
    private readonly NewApplicationDbContext _db;

    public PerfilService(IRepository<Perfil> repository, NewApplicationDbContext db)
    {
        _repository = repository;
        _db = db;
    }

    public async Task<IEnumerable<PerfilDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<PerfilDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<PerfilDetalleDto?> GetDetalleByIdAsync(int idPerfil)
    {
        if (idPerfil <= 0)
        {
            throw new ArgumentException("idPerfil debe ser mayor a 0.");
        }

        var perfil = await _db.Perfil
            .AsNoTracking()
            .Include(p => p.IdUniversidadNavigation)
            .FirstOrDefaultAsync(p => p.IdPerfil == idPerfil && p.Estado == "A");

        return perfil is null ? null : ToDetalleDto(perfil);
    }

    public async Task<PerfilDetalleDto?> GetDetalleByUsuarioIdAsync(int idUsuario)
    {
        if (idUsuario <= 0)
        {
            throw new ArgumentException("idUsuario debe ser mayor a 0.");
        }

        var perfil = await _db.Perfil
            .AsNoTracking()
            .Include(p => p.IdUniversidadNavigation)
            .FirstOrDefaultAsync(p => p.IdUsuario == idUsuario && p.Estado == "A");

        return perfil is null ? null : ToDetalleDto(perfil);
    }

    public async Task<PerfilDto> CreateAsync(PerfilCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, PerfilUpdateDto dto)
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

    private static PerfilDetalleDto ToDetalleDto(Perfil perfil)
    {
        return new PerfilDetalleDto
        {
            IdPerfil = perfil.IdPerfil,
            IdUsuario = perfil.IdUsuario,
            IdUbicacion = perfil.IdUbicacion,
            IdIdentificador = perfil.IdIdentificador,
            IdUniversidad = perfil.IdUniversidad,
            Identificacion = perfil.Identificacion,
            Nombre = perfil.Nombre,
            ApellidoP = perfil.ApellidoP,
            ApellidoM = perfil.ApellidoM,
            FechaNacimiento = perfil.FechaNacimiento,
            Carrera = perfil.Carrera,
            Bibliografia = perfil.Bibliografia,
            Estado = perfil.Estado[0],
            Universidad = perfil.IdUniversidadNavigation is null ? null : perfil.IdUniversidadNavigation.ToDto()
        };
    }
}
