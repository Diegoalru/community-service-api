using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.Entities;
using community_service_api.Repositories;

namespace community_service_api.Services;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllAsync();
    Task<UsuarioDto?> GetByIdAsync(Guid id);
    Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto);
    Task<bool> UpdateAsync(Guid id, UsuarioUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}

public class UsuarioService : IUsuarioService
{
    private readonly IRepository<Usuario> _repository;

    public UsuarioService(IRepository<Usuario> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<UsuarioDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(Guid id, UsuarioUpdateDto dto)
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
