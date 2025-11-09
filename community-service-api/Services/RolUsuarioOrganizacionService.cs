using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IRolUsuarioOrganizacionService
{
    Task<IEnumerable<RolUsuarioOrganizacionDto>> GetAllAsync();
    Task<RolUsuarioOrganizacionDto?> GetByIdAsync(int id);
    Task<RolUsuarioOrganizacionDto> CreateAsync(RolUsuarioOrganizacionCreateDto dto);
    Task<bool> UpdateAsync(int id, RolUsuarioOrganizacionUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class RolUsuarioOrganizacionService : IRolUsuarioOrganizacionService
{
    private readonly IRepository<RolUsuarioOrganizacion> _repository;

    public RolUsuarioOrganizacionService(IRepository<RolUsuarioOrganizacion> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RolUsuarioOrganizacionDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<RolUsuarioOrganizacionDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<RolUsuarioOrganizacionDto> CreateAsync(RolUsuarioOrganizacionCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, RolUsuarioOrganizacionUpdateDto dto)
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
}
