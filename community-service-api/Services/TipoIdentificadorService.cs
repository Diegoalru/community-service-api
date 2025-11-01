using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.Entities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface ITipoIdentificadorService
{
    Task<IEnumerable<TipoIdentificadorDto>> GetAllAsync();
    Task<TipoIdentificadorDto?> GetByIdAsync(int id);
    Task<TipoIdentificadorDto> CreateAsync(TipoIdentificadorCreateDto dto);
    Task<bool> UpdateAsync(int id, TipoIdentificadorUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class TipoIdentificadorService : ITipoIdentificadorService
{
    private readonly IRepository<TipoIdentificador> _repository;

    public TipoIdentificadorService(IRepository<TipoIdentificador> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TipoIdentificadorDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<TipoIdentificadorDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<TipoIdentificadorDto> CreateAsync(TipoIdentificadorCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, TipoIdentificadorUpdateDto dto)
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
