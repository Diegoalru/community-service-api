using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IOrganizacionService
{
    Task<IEnumerable<OrganizacionDto>> GetAllAsync();
    Task<OrganizacionDto?> GetByIdAsync(int id);
    Task<OrganizacionDto> CreateAsync(OrganizacionCreateDto dto);
    Task<bool> UpdateAsync(int id, OrganizacionUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class OrganizacionService : IOrganizacionService
{
    private readonly IRepository<Organizacion> _repository;

    public OrganizacionService(IRepository<Organizacion> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrganizacionDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<OrganizacionDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<OrganizacionDto> CreateAsync(OrganizacionCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, OrganizacionUpdateDto dto)
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
