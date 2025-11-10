using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.DBTableEntities;
using community_service_api.Models.Dtos;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IActividadService
{
    Task<IEnumerable<ActividadDto>> GetAllAsync();
    Task<ActividadDto?> GetByIdAsync(int id);
    Task<ActividadDto> CreateAsync(ActividadCreateDto dto);
    Task<bool> UpdateAsync(int id, ActividadUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class ActividadService : IActividadService
{
    private readonly IRepository<Actividad> _repository;

    public ActividadService(IRepository<Actividad> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ActividadDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
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
}
