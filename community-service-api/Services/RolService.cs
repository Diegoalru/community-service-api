using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.Entities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IRolService
{
    Task<IEnumerable<RolDto>> GetAllAsync();
    Task<RolDto?> GetByIdAsync(int id);
    Task<RolDto> CreateAsync(RolCreateDto dto);
    Task<bool> UpdateAsync(int id, RolUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class RolService : IRolService
{
    private readonly IRepository<Rol> _repository;

    public RolService(IRepository<Rol> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RolDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<RolDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<RolDto> CreateAsync(RolCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, RolUpdateDto dto)
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
