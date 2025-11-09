using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;
using Dapper.Oracle;

namespace community_service_api.Services;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllAsync();
    Task<UsuarioDto?> GetByIdAsync(int id);
    Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto);
    Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<int> CreateUsuarioWithProcedureAsync(UsuarioCreateDtoTest dto);
}

public class UsuarioService(IRepository<Usuario> repository, IProcedureRepository procedureRepository)
    : IUsuarioService
{
    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<UsuarioDto?> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<UsuarioDto> CreateAsync(UsuarioCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null)
        {
            return false;
        }

        entity.UpdateFromDto(dto);
        await repository.UpdateAsync(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }
    
    public async Task<int> CreateUsuarioWithProcedureAsync(UsuarioCreateDtoTest dto)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var dyParam = new OracleDynamicParameters();
            dyParam.Add("PV_USERNAME", dto.Username, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("PV_PASSWORD", dto.Password, OracleMappingType.Varchar2, ParameterDirection.Input);
            dyParam.Add("PV_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            var newUserId = await procedureRepository.ExecuteAsync<int>(
                "PKG_USUARIO.P_INSERTA_USUARIO",
                dyParam,
                "PV_ID_USUARIO");

            await procedureRepository.CommitTransactionAsync();
            return newUserId;
        }
        catch
        {
            await procedureRepository.RollbackTransactionAsync();
            throw;
        }
    }
}
