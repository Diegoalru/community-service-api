using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Exceptions;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;
using System.Data;
using Dapper.Oracle;

namespace community_service_api.Services;

public interface IParticipanteActividadService
{
    Task<IEnumerable<ParticipanteActividad>> GetAllAsync();
    Task<ParticipanteActividadDto?> GetByIdAsync(int id);
    Task<ParticipanteActividadDto> CreateAsync(ParticipanteActividadCreateDto dto);
    Task<bool> UpdateAsync(int id, ParticipanteActividadUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task CambiarSituacionAsync(int id, SituacionUpdateRequestDto dto, int idUsuarioSolicitante);
}

public class ParticipanteActividadService : IParticipanteActividadService
{
    private readonly IRepository<ParticipanteActividad> _repository;
    private readonly IProcedureRepository _procedureRepository;

    public ParticipanteActividadService(IRepository<ParticipanteActividad> repository, IProcedureRepository procedureRepository)
    {
        _repository = repository;
        _procedureRepository = procedureRepository;
    }

    public async Task<IEnumerable<ParticipanteActividad>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
        //return entities.Select(e => e.ToDto());
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
    
    public async Task CambiarSituacionAsync(int id, SituacionUpdateRequestDto dto, int idUsuarioSolicitante)
    {
        // Usar OracleDynamicParameters/OracleMappingType para evitar errores con Dapper
        var parameters = new OracleDynamicParameters();
        parameters.Add("PN_ID_PARTICIPANTE_ACTIVIDAD", id, OracleMappingType.Int32, ParameterDirection.Input);
        parameters.Add("PV_SITUACION", dto.Situacion.ToString(), OracleMappingType.Varchar2, ParameterDirection.Input);
        parameters.Add("PN_ID_USUARIO_SOLICITANTE", idUsuarioSolicitante, OracleMappingType.Int32, ParameterDirection.Input);
        parameters.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        parameters.Add("PV_MENSAJE_ERROR", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);

        // Ejecutar el procedimiento y considerar PN_EXITO como salida principal
        await _procedureRepository.ExecuteAsync<int>(
            "PKG_PARTICIPANTE_ACTIVIDAD.P_CAMBIAR_SITUACION_PARTICIPANTE",
            parameters,
            outputParameterName: "PN_EXITO");

        var exito = parameters.Get<int?>("PN_EXITO") ?? 0;
        if (exito != 1)
        {
            var errorMessage = parameters.Get<string>("PV_MENSAJE_ERROR") ?? "Ocurrió un error al cambiar la situación.";
            throw new BusinessRuleException(errorMessage);
        }
    }
}
