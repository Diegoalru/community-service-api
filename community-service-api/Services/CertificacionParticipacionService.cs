using System.Data;
using community_service_api.Helpers;
using community_service_api.Models.DBTableEntities;
using community_service_api.Models.Dtos;
using community_service_api.Repositories;
using Dapper.Oracle;

namespace community_service_api.Services;

public interface ICertificacionParticipacionService
{
    Task<IEnumerable<CertificacionParticipacionDto>> GetAllAsync();
    Task<CertificacionParticipacionDto?> GetByIdAsync(Guid id);
    Task<CertificacionParticipacionDto> CreateAsync(CertificacionParticipacionCreateDto dto);
    Task<bool> UpdateAsync(Guid id, CertificacionParticipacionUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<CertificacionParticipacionDto>> GetPendingAsync();
    Task SaveCertificateDocumentAsync(Guid idCertificacion, byte[] documento, CancellationToken cancellationToken);
    Task<IEnumerable<CertificatePdfDataDto>> GetCertificatePdfDataAsync();
    Task<IEnumerable<CertificateEmailDataDto>> GetCertificateEmailDataAsync();
    Task UpdateSendStatusAsync(Guid idCertificacion, bool envioExitoso, string? errorEnvio);
}

public class CertificacionParticipacionService(
    IRepository<CertificadoParticipacion> repository,
    IProcedureRepository procedureRepository)
    : ICertificacionParticipacionService
{
    public async Task<IEnumerable<CertificacionParticipacionDto>> GetAllAsync()
    {
        var entities = await repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<CertificacionParticipacionDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<CertificacionParticipacionDto> CreateAsync(CertificacionParticipacionCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(Guid id, CertificacionParticipacionUpdateDto dto)
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

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CertificacionParticipacionDto>> GetPendingAsync() 
    {
        var entities = await repository.GetAllAsync();

        var pendingEntities = entities.Where(e => e.Situacion.Equals("p", StringComparison.InvariantCultureIgnoreCase));

        return pendingEntities.Select(e => e.ToDto());
    }

    public async Task SaveCertificateDocumentAsync(Guid idCertificacion, byte[] documento, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await procedureRepository.BeginTransactionAsync();

        try
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("PR_ID_CERTIFICACION", idCertificacion.ToByteArray(), OracleMappingType.Raw, ParameterDirection.Input);
            parameters.Add("PB_DOCUMENTO", documento, OracleMappingType.Blob, ParameterDirection.Input);
            parameters.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            parameters.Add("PV_MENSAJE_ERROR", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);

            await procedureRepository.ExecuteAsync<int>(
                "PKG_CERTIFICADO_PARTICIPACION.P_ADJUNTAR_DOCUMENTO",
                parameters,
                "PN_EXITO");

            await procedureRepository.CommitTransactionAsync();
        }
        catch
        {
            await procedureRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<CertificatePdfDataDto>> GetCertificatePdfDataAsync()
    {
        var parameters = new OracleDynamicParameters();
        parameters.Add("PC_DATOS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        parameters.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        parameters.Add("PV_MENSAJE_ERROR", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);

        return await procedureRepository.QueryAsync<CertificatePdfDataDto>(
            "PKG_CERTIFICADO_PARTICIPACION.P_OBTENER_DATOS_PDF",
            parameters);
    }

    public async Task<IEnumerable<CertificateEmailDataDto>> GetCertificateEmailDataAsync()
    {
        var parameters = new OracleDynamicParameters();
        parameters.Add("PC_DATOS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        parameters.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        parameters.Add("PV_MENSAJE_ERROR", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);

        return await procedureRepository.QueryAsync<CertificateEmailDataDto>(
            "PKG_CERTIFICADO_PARTICIPACION.P_OBTENER_DATOS_USUARIOS_ENVIO",
            parameters);
    }

    public async Task UpdateSendStatusAsync(Guid idCertificacion, bool envioExitoso, string? errorEnvio)
    {
        var parameters = new OracleDynamicParameters();
        parameters.Add("PR_ID_CERTIFICACION", idCertificacion.ToByteArray(), OracleMappingType.Raw, ParameterDirection.Input);
        parameters.Add("PN_ENVIO_EXITOSO", envioExitoso ? 1 : 0, OracleMappingType.Int32, ParameterDirection.Input);
        parameters.Add("PV_ERROR_ENVIO", errorEnvio, OracleMappingType.Varchar2, ParameterDirection.Input);
        parameters.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        parameters.Add("PV_MENSAJE_ERROR", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);

        await procedureRepository.ExecuteAsync<int>(
            "PKG_CERTIFICADO_PARTICIPACION.P_ACTUALIZAR_ESTADO_ENVIO",
            parameters,
            "PN_EXITO");
    }
}