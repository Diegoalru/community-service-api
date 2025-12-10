using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IUniversidadService
{
    Task<IEnumerable<UniversidadDto>> GetAllAsync();
    Task<UniversidadDto?> GetByIdAsync(int id);
    Task<UniversidadDto> CreateAsync(UniversidadCreateDto dto);
    Task<bool> UpdateAsync(int id, UniversidadUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class UniversidadService : IUniversidadService
{
    private readonly IRepository<Universidad> _repository;

    public UniversidadService(IRepository<Universidad> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UniversidadDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<UniversidadDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<UniversidadDto> CreateAsync(UniversidadCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, UniversidadUpdateDto dto)
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

