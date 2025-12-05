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

public class CertificacionParticipacionService : ICertificacionParticipacionService
{
    private readonly IRepository<CertificadoParticipacion> _repository;
    private readonly IProcedureRepository _procedureRepository;

    public CertificacionParticipacionService(
        IRepository<CertificadoParticipacion> repository,
        IProcedureRepository procedureRepository)
    {
        _repository = repository;
        _procedureRepository = procedureRepository;
    }

    public async Task<IEnumerable<CertificacionParticipacionDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e => e.ToDto());
    }

    public async Task<CertificacionParticipacionDto?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<CertificacionParticipacionDto> CreateAsync(CertificacionParticipacionCreateDto dto)
    {
        var entity = dto.ToEntity();
        var created = await _repository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task<bool> UpdateAsync(Guid id, CertificacionParticipacionUpdateDto dto)
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

    public async Task<IEnumerable<CertificacionParticipacionDto>> GetPendingAsync() 
    {
        var entities = await _repository.GetAllAsync();

        var pendingEntities = entities.Where(e => e.Situacion.Equals("p", StringComparison.InvariantCultureIgnoreCase));

        return pendingEntities.Select(e => e.ToDto());
    }

    public async Task SaveCertificateDocumentAsync(Guid idCertificacion, byte[] documento, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Anonymous PL/SQL block - BOOLEAN params stay as local variables, not bound to .NET
        const string plsqlBlock = @"
            DECLARE
                l_exito BOOLEAN;
                l_mensaje VARCHAR2(4000);
            BEGIN
                PKG_CERTIFICADO_PARTICIPACION.P_ADJUNTAR_DOCUMENTO(
                    PR_ID_CERTIFICACION => :PR_ID_CERTIFICACION,
                    PB_DOCUMENTO => :PB_DOCUMENTO,
                    PB_EXITO => l_exito,
                    PV_MENSAJE_ERROR => l_mensaje
                );
            END;";

        await _procedureRepository.BeginTransactionAsync();

        try
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("PR_ID_CERTIFICACION", idCertificacion.ToByteArray(), OracleMappingType.Raw, ParameterDirection.Input);
            parameters.Add("PB_DOCUMENTO", documento, OracleMappingType.Blob, ParameterDirection.Input);

            await _procedureRepository.ExecuteAnonymousBlockAsync(plsqlBlock, parameters);

            await _procedureRepository.CommitTransactionAsync();
        }
        catch
        {
            await _procedureRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<CertificatePdfDataDto>> GetCertificatePdfDataAsync()
    {
        // Anonymous PL/SQL block - BOOLEAN params stay as local variables, not bound to .NET
        const string plsqlBlock = @"
            DECLARE
                l_exito BOOLEAN;
                l_mensaje VARCHAR2(4000);
            BEGIN
                PKG_CERTIFICADO_PARTICIPACION.P_OBTENER_DATOS_PDF(
                    PC_DATOS => :PC_DATOS,
                    PB_EXITO => l_exito,
                    PV_MENSAJE_ERROR => l_mensaje
                );
            END;";

        var parameters = new OracleDynamicParameters();
        parameters.Add("PC_DATOS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

        return await _procedureRepository.QueryWithAnonymousBlockAsync<CertificatePdfDataDto>(plsqlBlock, parameters);
    }

    public async Task<IEnumerable<CertificateEmailDataDto>> GetCertificateEmailDataAsync()
    {
        // Anonymous PL/SQL block - BOOLEAN params stay as local variables, not bound to .NET
        const string plsqlBlock = @"
            DECLARE
                l_exito BOOLEAN;
                l_mensaje VARCHAR2(4000);
            BEGIN
                PKG_CERTIFICADO_PARTICIPACION.P_OBTENER_DATOS_USUARIOS_ENVIO(
                    PC_DATOS => :PC_DATOS,
                    PB_EXITO => l_exito,
                    PV_MENSAJE_ERROR => l_mensaje
                );
            END;";

        var parameters = new OracleDynamicParameters();
        parameters.Add("PC_DATOS", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

        return await _procedureRepository.QueryWithAnonymousBlockAsync<CertificateEmailDataDto>(plsqlBlock, parameters);
    }

    public async Task UpdateSendStatusAsync(Guid idCertificacion, bool envioExitoso, string? errorEnvio)
    {
        // Anonymous PL/SQL block - BOOLEAN params converted from NUMBER inside PL/SQL
        const string plsqlBlock = @"
            DECLARE
                l_exito BOOLEAN;
                l_mensaje VARCHAR2(4000);
            BEGIN
                PKG_CERTIFICADO_PARTICIPACION.P_ACTUALIZAR_ESTADO_ENVIO(
                    PR_ID_CERTIFICACION => :PR_ID_CERTIFICACION,
                    PB_ENVIO_EXITOSO => CASE WHEN :PB_ENVIO_EXITOSO = 1 THEN TRUE ELSE FALSE END,
                    PV_ERROR_ENVIO => :PV_ERROR_ENVIO,
                    PB_EXITO => l_exito,
                    PV_MENSAJE_ERROR => l_mensaje
                );
            END;";

        var parameters = new OracleDynamicParameters();
        parameters.Add("PR_ID_CERTIFICACION", idCertificacion.ToByteArray(), OracleMappingType.Raw, ParameterDirection.Input);
        parameters.Add("PB_ENVIO_EXITOSO", envioExitoso ? 1 : 0, OracleMappingType.Int32, ParameterDirection.Input);
        parameters.Add("PV_ERROR_ENVIO", errorEnvio, OracleMappingType.Varchar2, ParameterDirection.Input);

        await _procedureRepository.ExecuteAnonymousBlockAsync(plsqlBlock, parameters);
    }
}
