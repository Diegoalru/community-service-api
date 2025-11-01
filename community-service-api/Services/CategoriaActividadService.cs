using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.Entities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface ICategoriaActividadService
{
    Task<IEnumerable<CategoriaActividadDto>> GetAllAsync();
    Task<CategoriaActividadDto?> GetByIdAsync(int id);
    Task<CategoriaActividadDto> CreateAsync(CategoriaActividadCreateDto dto);
    Task<bool> UpdateAsync(int id, CategoriaActividadUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class CategoriaActividadService : ICategoriaActividadService
{
    private readonly IRepository<CategoriaActividad> _repository;

    public CategoriaActividadService(IRepository<CategoriaActividad> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoriaActividadDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<CategoriaActividadDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<CategoriaActividadDto> CreateAsync(CategoriaActividadCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, CategoriaActividadUpdateDto dto)
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
