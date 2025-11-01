using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.Entities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IPerfilService
{
    Task<IEnumerable<PerfilDto>> GetAllAsync();
    Task<PerfilDto?> GetByIdAsync(Guid id);
    Task<PerfilDto> CreateAsync(PerfilCreateDto dto);
    Task<bool> UpdateAsync(Guid id, PerfilUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}

public class PerfilService : IPerfilService
{
    private readonly IRepository<Perfil> _repository;

    public PerfilService(IRepository<Perfil> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PerfilDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<PerfilDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<PerfilDto> CreateAsync(PerfilCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(Guid id, PerfilUpdateDto dto)
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

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
