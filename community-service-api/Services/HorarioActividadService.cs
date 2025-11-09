using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IHorarioActividadService
{
    Task<IEnumerable<HorarioActividadDto>> GetAllAsync();
    Task<HorarioActividadDto?> GetByIdAsync(Guid id);
    Task<HorarioActividadDto> CreateAsync(HorarioActividadCreateDto dto);
    Task<bool> UpdateAsync(Guid id, HorarioActividadUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}

public class HorarioActividadService : IHorarioActividadService
{
    private readonly IRepository<HorarioActividad> _repository;

    public HorarioActividadService(IRepository<HorarioActividad> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<HorarioActividadDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<HorarioActividadDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<HorarioActividadDto> CreateAsync(HorarioActividadCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(Guid id, HorarioActividadUpdateDto dto)
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
