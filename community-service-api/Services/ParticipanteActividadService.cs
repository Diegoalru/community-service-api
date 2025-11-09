using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IParticipanteActividadService
{
    Task<IEnumerable<ParticipanteActividadDto>> GetAllAsync();
    Task<ParticipanteActividadDto?> GetByIdAsync(int id);
    Task<ParticipanteActividadDto> CreateAsync(ParticipanteActividadCreateDto dto);
    Task<bool> UpdateAsync(int id, ParticipanteActividadUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class ParticipanteActividadService : IParticipanteActividadService
{
    private readonly IRepository<ParticipanteActividad> _repository;

    public ParticipanteActividadService(IRepository<ParticipanteActividad> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ParticipanteActividadDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<ParticipanteActividadDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<ParticipanteActividadDto> CreateAsync(ParticipanteActividadCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, ParticipanteActividadUpdateDto dto)
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
