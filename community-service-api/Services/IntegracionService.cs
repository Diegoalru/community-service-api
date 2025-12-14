using System.Data;
using System.Text.Json;
using community_service_api.MailTemplates;
using community_service_api.Models;
using community_service_api.Models.Dtos;
using community_service_api.Repositories;
using Dapper.Oracle;
using Microsoft.Extensions.Options;

namespace community_service_api.Services;

public interface IIntegracionService
{
    Task<Respuesta> RegistroUsuarioCompletoAsync(RegistroCompletoDto dto);
    Task<Respuesta> InicioSesionAsync(UsuarioLoginDto dto);
    Task<Respuesta> ActivarCuentaAsync(string token);
    Task<Respuesta> SolicitarRecuperacionPasswordAsync(RequestPasswordRecoveryDto dto);
    Task<Respuesta> RestablecerPasswordAsync(ResetPasswordDto dto);
    Task<Respuesta> CambiarPasswordAsync(ChangePasswordDto dto);
    Task<Respuesta> ReenviarActivacionAsync(ResendActivationDto dto);
}

public class IntegracionService(
    IProcedureRepository procedureRepository,
    IEmailQueueService emailQueueService,
    IOptions<FrontEndSettings> frontEndSettings)
    : IIntegracionService
{
    private readonly FrontEndSettings _frontEndSettings = frontEndSettings.Value;

    public async Task<Respuesta> RegistroUsuarioCompletoAsync(RegistroCompletoDto dto)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = new OracleDynamicParameters();

            dyParam.Add("PV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);

            dyParam.Add("PN_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PV_TOKEN", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_REGISTRO_USUARIO_JSON", dyParam);

            var response = new Respuesta
            {
                IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
                Token = dyParam.Get<string>("PV_TOKEN"),
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty,
                Codigo = dyParam.Get<int>("PN_CODIGO")
            };

            if (response.Exito == 1)
            {
                await procedureRepository.CommitTransactionAsync();

                // Enviar email DESPUÉS de hacer commit - EN UN BLOQUE SEPARADO
                if (!string.IsNullOrEmpty(response.Token))
                {
                    var email = dto.Correspondencia.FirstOrDefault(c => c.IdTipoCorrespondencia == 1)?.Valor;
                    if (!string.IsNullOrEmpty(email))
                        try
                        {
                            var activationLink = $"{_frontEndSettings.Url}/activate?token={response.Token}";
                            var body = ActivationMailTemplate.GetBody(activationLink);
                            await emailQueueService.EnqueueEmailAsync(email, "Activación de Cuenta", body,
                                response.IdUsuario!.Value);
                        }
                        catch (Exception emailEx)
                        {
                            // Log del error pero no afecta la respuesta del registro
                            Console.WriteLine($"Error enviando email: {emailEx.Message}");
                        }
                }
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al registrar el usuario. {ex.Message}"
            };
        }
    }


    public async Task<Respuesta> InicioSesionAsync(UsuarioLoginDto dto)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("PV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);

            dyParam.Add("PN_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_INICIO_SESION_JSON", dyParam);

            var response = new Respuesta
            {
                IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty
            };

            await procedureRepository.CommitTransactionAsync();

            return response;
        }
        catch
        {
            await procedureRepository.RollbackTransactionAsync();

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = "Ocurrió un error al iniciar sesión."
            };
        }
    }

    public async Task<Respuesta> ActivarCuentaAsync(string token)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(new { token });

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("PV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);

            dyParam.Add("PN_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_VERIFICAR_TOKEN_JSON", dyParam);

            var response = new Respuesta
            {
                IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
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

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al activar la cuenta. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> SolicitarRecuperacionPasswordAsync(RequestPasswordRecoveryDto dto)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("PV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);
            dyParam.Add("PV_TOKEN", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PV_EMAIL", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_SOLICITAR_RECUPERACION_JSON", dyParam);

            var response = new Respuesta
            {
                Token = dyParam.Get<string>("PV_TOKEN"),
                Email = dyParam.Get<string>("PV_EMAIL"),
                IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty,
                Codigo = dyParam.Get<int>("PN_CODIGO")
            };

            if (response.Exito == 1)
            {
                await procedureRepository.CommitTransactionAsync();

                // Enviar email DESPUÉS de hacer commit
                if (!string.IsNullOrEmpty(response.Token) && !string.IsNullOrEmpty(response.Email))
                {
                    var recoveryLink = $"{_frontEndSettings.Url}/reset-password?token={response.Token}";
                    var body = PasswordRecoveryMailTemplate.GetBody(recoveryLink);
                    await emailQueueService.EnqueueEmailAsync(response.Email, "Recuperación de Contraseña", body,
                        response.IdUsuario!.Value);
                }
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al solicitar la recuperación de la cuenta. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> RestablecerPasswordAsync(ResetPasswordDto dto)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("PV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);

            dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_RESTABLECER_PASSWORD_JSON", dyParam);

            var response = new Respuesta
            {
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

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al restablecer la contraseña. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> CambiarPasswordAsync(ChangePasswordDto dto)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("PV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);

            dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_CAMBIAR_PASSWORD_JSON", dyParam);

            var response = new Respuesta
            {
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

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al cambiar la contraseña. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> ReenviarActivacionAsync(ResendActivationDto dto)
    {
        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = new OracleDynamicParameters();
            dyParam.Add("PV_DATOS", jsonPayload, OracleMappingType.Clob, ParameterDirection.Input);

            dyParam.Add("PV_EMAIL", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PV_TOKEN", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_ID_USUARIO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PV_MENSAJE", dbType: OracleMappingType.Varchar2, size: 2000,
                direction: ParameterDirection.Output);
            dyParam.Add("PN_CODIGO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
            dyParam.Add("PN_EXITO", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_REENVIAR_ACTIVACION_JSON", dyParam);

            var response = new Respuesta
            {
                Email = dyParam.Get<string>("PV_EMAIL"),
                Token = dyParam.Get<string>("PV_TOKEN"),
                IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty,
                Codigo = dyParam.Get<int>("PN_CODIGO")
            };

            if (response.Exito == 1)
            {
                await procedureRepository.CommitTransactionAsync();

                // Enviar email DESPUÉS de hacer commit
                if (!string.IsNullOrEmpty(response.Token) && !string.IsNullOrEmpty(response.Email))
                {
                    var activationLink = $"{_frontEndSettings.Url}/activate?token={response.Token}";
                    var body = ActivationMailTemplate.GetBody(activationLink);
                    await emailQueueService.EnqueueEmailAsync(response.Email, "Activación de Cuenta", body,
                        response.IdUsuario!.Value);
                }
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al reenviar la activación. {ex.Message}"
            };
        }
    }
}

// Clases de respuesta auxiliares
public class Respuesta
{
    public int Codigo { get; set; }
    public int Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public int? IdUsuario { get; set; }
    public string? Token { get; set; }
    public string? Email { get; set; }
}