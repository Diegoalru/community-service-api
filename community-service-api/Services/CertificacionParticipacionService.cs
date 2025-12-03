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
    Task SaveCertificateDocumentAsync(Guid idCertificacion, byte[] documento, CancellationToken cancellationToken);
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

    public async Task SaveCertificateDocumentAsync(Guid idCertificacion, byte[] documento, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _procedureRepository.BeginTransactionAsync();

        try
        {
            var parameters = new OracleDynamicParameters();
            parameters.Add("PR_ID_CERTIFICACION", idCertificacion.ToByteArray(), OracleMappingType.Raw, ParameterDirection.Input);
            parameters.Add("PB_DOCUMENTO", documento, OracleMappingType.Blob, ParameterDirection.Input);
            parameters.Add("PB_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            parameters.Add("PV_MENSAJE_ERROR", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output);

            await _procedureRepository.ExecuteAsync<string>(
                "P_ADJUNTAR_DOCUMENTO",
                parameters,
                "PV_MENSAJE_ERROR");

            await _procedureRepository.CommitTransactionAsync();
        }
        catch
        {
            await _procedureRepository.RollbackTransactionAsync();
            throw;
        }
    }
}
