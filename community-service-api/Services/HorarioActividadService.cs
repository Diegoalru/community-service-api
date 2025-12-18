using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using community_service_api.Exceptions;
using community_service_api.Helpers;
using community_service_api.Models.Dtos;
using community_service_api.Models.DBTableEntities;
using community_service_api.Repositories;
using Dapper.Oracle;
using Oracle.ManagedDataAccess.Client;

namespace community_service_api.Services;

public interface IHorarioActividadService
{
    Task<IEnumerable<HorarioActividadDto>> GetAllAsync();
    Task<HorarioActividadDto?> GetByIdAsync(int id);
    Task<HorarioActividadDto> CreateAsync(HorarioActividadCreateDto dto);
    Task<bool> UpdateAsync(int id, HorarioActividadUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ParticipanteActividadDisplayDto>> GetParticipantesByHorarioAsync(int horarioId);
}

public class HorarioActividadService(IRepository<HorarioActividad> repository, IProcedureRepository procedureRepository)
    : IHorarioActividadService
{
    public async Task<IEnumerable<HorarioActividadDto>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<HorarioActividadDto?> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<HorarioActividadDto> CreateAsync(HorarioActividadCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, HorarioActividadUpdateDto dto)
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

    public async Task<IEnumerable<ParticipanteActividadDisplayDto>> GetParticipantesByHorarioAsync(int horarioId)
    {
        var parameters = new OracleDynamicParameters();
        parameters.Add("PN_ID_HORARIO_ACTIVIDAD", horarioId, OracleMappingType.Int32, ParameterDirection.Input);
        parameters.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        parameters.Add("PV_MENSAJE_ERROR", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);
        parameters.Add("PC_RESULTADOS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

        var result = await procedureRepository.QueryAsync<ParticipanteActividadDisplayDto>(
            "PKG_PARTICIPANTE_ACTIVIDAD.P_GET_PARTICIPANTES_BY_HORARIO", parameters);

        var exito = parameters.Get<int?>("PN_EXITO") ?? 0;
        var mensaje = parameters.Get<string?>("PV_MENSAJE_ERROR");

        if (exito == 1) return result;

        var errorMessage = !string.IsNullOrWhiteSpace(mensaje)
            ? mensaje!
            : "Ocurrió un error al obtener los participantes del horario.";
        throw new BusinessRuleException(errorMessage);
    }
}
