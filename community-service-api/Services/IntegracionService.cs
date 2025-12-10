using System.Data;
using System.Text.Json;
using community_service_api.Models.Dtos;
using community_service_api.Repositories;
using Dapper.Oracle;

namespace community_service_api.Services;

public interface IIntegracionService
{
    Task<RespuestaRegistro> RegistrarUsuarioCompletoAsync(RegistroCompletoDto dto);
}

public class IntegracionService(IProcedureRepository procedureRepository) : IIntegracionService
{
    public async Task<RespuestaRegistro> RegistrarUsuarioCompletoAsync(RegistroCompletoDto dto)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = new OracleDynamicParameters();
            
            dyParam.Add("CV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);

            dyParam.Add("PN_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PV_TOKEN", dbType: OracleMappingType.Varchar2, size: 2000, direction: ParameterDirection.Output);
            dyParam.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000, direction: ParameterDirection.Output);
            dyParam.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_REGISTRO_USUARIO_JSON", dyParam);

            var response = new RespuestaRegistro
            {
                IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
                Token = dyParam.Get<string>("PV_TOKEN"),
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty,
                Codigo = dyParam.Get<int>("PN_CODIGO")
            };

            await procedureRepository.CommitTransactionAsync();

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();

            return new RespuestaRegistro
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al registrar el usuario. {ex.Message}"
            };
        }
    }
}

// Clases de respuesta auxiliares
public class RespuestaRegistro
{
    public int Codigo { get; set; }
    public int Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public int? IdUsuario { get; set; }
    public string? Token { get; set; }
}

public class RespuestaReenvio
{
    public int Codigo { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Token { get; set; }
}