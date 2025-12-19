using System.Text.Json;
using community_service_api.MailTemplates;
using community_service_api.Models;
using community_service_api.Models.Dtos;
using community_service_api.Repositories;
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
    Task<Respuesta> CrearOrganizacionAsync(OrganizacionCreacionDto dto);
    Task<Respuesta> CrearActividadAsync(ActividadCreacionIntegracionDto dto);
    Task<Respuesta> CrearHorarioAsync(HorarioCreacionIntegracionDto dto);
    Task<Respuesta> InscribirParticipanteAsync(InscripcionParticipanteDto dto);
    Task<Respuesta> AsignarRolAsync(AsignacionRolDto dto);
    Task<Respuesta> ActualizarPerfilCompletoAsync(ActualizacionPerfilCompletoDto dto);
    Task<Respuesta> ActualizarActividadAsync(ActividadActualizacionIntegracionDto dto);
}

public class IntegracionService(
    IProcedureRepository procedureRepository,
    IEmailQueueService emailQueueService,
    IJwtService jwtService,
    ILogger<IntegracionService> logger,
    IOptions<FrontEndSettings> frontEndSettings)
    : IIntegracionService
{
    private readonly FrontEndSettings _frontEndSettings = frontEndSettings.Value;

    public async Task<Respuesta> RegistroUsuarioCompletoAsync(RegistroCompletoDto dto)
    {
        logger.LogInformation("Iniciando registro de usuario: {Username}", dto.Usuario.Username);

        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputIdUsuario()
                .WithOutputToken()
                .WithStandardOutputs()
                .Build();

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
                logger.LogInformation("Usuario registrado exitosamente. ID: {IdUsuario}, Username: {Username}",
                    response.IdUsuario, dto.Usuario.Username);

                // Enviar email DESPUÉS de hacer commit - EN UN BLOQUE SEPARADO
                if (string.IsNullOrEmpty(response.Token)) return response;

                var email = dto.Correspondencia.FirstOrDefault(c => c.IdTipoCorrespondencia == 1)?.Valor;

                if (string.IsNullOrEmpty(email)) return response;

                try
                {
                    var activationLink = $"{_frontEndSettings.Url}/activate?token={response.Token}";
                    var body = ActivationMailTemplate.GetBody(activationLink);
                    await emailQueueService.EnqueueEmailAsync(email, "Activación de Cuenta", body,
                        response.IdUsuario!.Value);
                    logger.LogInformation("Email de activación encolado para: {Email}", email);
                }
                catch (Exception emailEx)
                {
                    logger.LogWarning(emailEx, "Error encolando email de activación para: {Email}", email);
                }
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning(
                    "Registro de usuario fallido. Username: {Username}, Código: {Codigo}, Mensaje: {Mensaje}",
                    dto.Usuario.Username, response.Codigo, response.Mensaje);
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en registro de usuario: {Username}", dto.Usuario.Username);

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al registrar el usuario. {ex.Message}"
            };
        }
    }


    public async Task<Respuesta> InicioSesionAsync(UsuarioLoginDto dto)
    {
        logger.LogInformation("Intento de inicio de sesión para: {Username}", dto.Username);

        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputIdUsuario()
                .WithBasicOutputs()
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_INICIO_SESION_JSON", dyParam);

            var response = new Respuesta
            {
                IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty
            };

            await procedureRepository.CommitTransactionAsync();

            // Generar JWT si el login fue exitoso
            if (response.Exito == 1 && response.IdUsuario.HasValue)
            {
                response.Token = jwtService.GenerateToken(response.IdUsuario.Value, dto.Username);
                logger.LogInformation("Inicio de sesión exitoso para: {Username} (ID: {IdUsuario})",
                    dto.Username, response.IdUsuario);
            }
            else
            {
                logger.LogWarning("Inicio de sesión fallido para: {Username}. Mensaje: {Mensaje}",
                    dto.Username, response.Mensaje);
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en inicio de sesión para: {Username}", dto.Username);

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = "Ocurrió un error al iniciar sesión."
            };
        }
    }

    public async Task<Respuesta> ActivarCuentaAsync(string token)
    {
        logger.LogInformation("Intento de activación de cuenta con token");

        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(new { token });

            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputIdUsuario()
                .WithStandardOutputs()
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_VERIFICAR_TOKEN_JSON", dyParam);

            var response = new Respuesta
            {
                IdUsuario = dyParam.Get<int?>("PN_ID_USUARIO"),
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty,
                Codigo = dyParam.Get<int>("PN_CODIGO")
            };

            if (response.Exito == 1)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Cuenta activada exitosamente. ID Usuario: {IdUsuario}", response.IdUsuario);
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Activación de cuenta fallida. Código: {Codigo}, Mensaje: {Mensaje}",
                    response.Codigo, response.Mensaje);
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en activación de cuenta");

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al activar la cuenta. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> SolicitarRecuperacionPasswordAsync(RequestPasswordRecoveryDto dto)
    {
        logger.LogInformation("Solicitud de recuperación de contraseña para: {Username}", dto.Username);

        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputToken()
                .WithOutputEmail()
                .WithOutputIdUsuario()
                .WithStandardOutputs()
                .Build();

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
                logger.LogInformation("Token de recuperación generado para usuario ID: {IdUsuario}",
                    response.IdUsuario);

                // Enviar email DESPUÉS de hacer commit
                if (string.IsNullOrEmpty(response.Token) || string.IsNullOrEmpty(response.Email)) return response;

                var recoveryLink = $"{_frontEndSettings.Url}/reset-password?token={response.Token}";
                var body = PasswordRecoveryMailTemplate.GetBody(recoveryLink);
                await emailQueueService.EnqueueEmailAsync(response.Email, "Recuperación de Contraseña", body,
                    response.IdUsuario!.Value);
                logger.LogInformation("Email de recuperación encolado para: {Email}", response.Email);
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                // No logueamos username para no revelar si existe o no
                logger.LogDebug("Solicitud de recuperación fallida. Código: {Codigo}", response.Codigo);
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en solicitud de recuperación de contraseña");

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al solicitar la recuperación de la cuenta. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> RestablecerPasswordAsync(ResetPasswordDto dto)
    {
        logger.LogInformation("Intento de restablecimiento de contraseña con token");

        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithStandardOutputs()
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_RESTABLECER_PASSWORD_JSON", dyParam);

            var response = new Respuesta
            {
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty,
                Codigo = dyParam.Get<int>("PN_CODIGO")
            };

            if (response.Exito == 1)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Contraseña restablecida exitosamente");
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Restablecimiento de contraseña fallido. Código: {Codigo}, Mensaje: {Mensaje}",
                    response.Codigo, response.Mensaje);
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en restablecimiento de contraseña");

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al restablecer la contraseña. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> CambiarPasswordAsync(ChangePasswordDto dto)
    {
        logger.LogInformation("Intento de cambio de contraseña para: {Username}", dto.Username);

        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithStandardOutputs()
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_CAMBIAR_PASSWORD_JSON", dyParam);

            var response = new Respuesta
            {
                Exito = dyParam.Get<int>("PN_EXITO"),
                Mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty,
                Codigo = dyParam.Get<int>("PN_CODIGO")
            };

            if (response.Exito == 1)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Contraseña cambiada exitosamente para: {Username}", dto.Username);
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Cambio de contraseña fallido para: {Username}. Código: {Codigo}",
                    dto.Username, response.Codigo);
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en cambio de contraseña para: {Username}", dto.Username);

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al cambiar la contraseña. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> ReenviarActivacionAsync(ResendActivationDto dto)
    {
        logger.LogInformation("Solicitud de reenvío de activación para: {Username}", dto.Username);

        await procedureRepository.BeginTransactionAsync();

        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);

            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputEmail()
                .WithOutputToken()
                .WithOutputIdUsuario()
                .WithStandardOutputs()
                .Build();

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
                logger.LogInformation("Token de activación regenerado para usuario ID: {IdUsuario}",
                    response.IdUsuario);

                // Enviar email DESPUÉS de hacer commit
                if (string.IsNullOrEmpty(response.Token) || string.IsNullOrEmpty(response.Email)) return response;
                var activationLink = $"{_frontEndSettings.Url}/activate?token={response.Token}";
                var body = ActivationMailTemplate.GetBody(activationLink);
                await emailQueueService.EnqueueEmailAsync(response.Email, "Activación de Cuenta", body,
                    response.IdUsuario!.Value);
                logger.LogInformation("Email de activación reenviado a: {Email}", response.Email);
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogDebug("Reenvío de activación fallido. Código: {Codigo}", response.Codigo);
            }

            return response;
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en reenvío de activación para: {Username}", dto.Username);

            return new Respuesta
            {
                Codigo = -1,
                Mensaje = $"Ocurrió un error al reenviar la activación. {ex.Message}"
            };
        }
    }

    public async Task<Respuesta> CrearOrganizacionAsync(OrganizacionCreacionDto dto)
    {
        logger.LogInformation("Iniciando creación de organización: {NombreOrganizacion}", dto.Nombre);

        await procedureRepository.BeginTransactionAsync();
        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);
            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputNumber("PN_ID_ORGANIZACION")
                .WithOutputVarchar("PV_MENSAJE", 4000)
                .WithOutputNumber("PN_CODIGO")
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_CREAR_ORGANIZACION_JSON", dyParam);

            var idOrganizacion = dyParam.Get<int?>("PN_ID_ORGANIZACION");
            var mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty;
            var codigo = dyParam.Get<int?>("PN_CODIGO") ?? 0;

            if (idOrganizacion.HasValue && idOrganizacion > 0)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Organización creada exitosamente: {NombreOrganizacion}, ID: {IdOrganizacion}", dto.Nombre, idOrganizacion);
                return new Respuesta { Exito = 1, Mensaje = mensaje, IdEntidad = idOrganizacion };
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Creación de organización fallida: {NombreOrganizacion}, Código: {Codigo}, Mensaje: {Mensaje}", dto.Nombre, codigo, mensaje);
                return new Respuesta { Exito = 0, Codigo = codigo, Mensaje = mensaje };
            }
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en creación de organización: {NombreOrganizacion}", dto.Nombre);
            return new Respuesta { Codigo = -1, Mensaje = $"Ocurrió un error al crear la organización. {ex.Message}" };
        }
    }

    public async Task<Respuesta> CrearActividadAsync(ActividadCreacionIntegracionDto dto)
    {
        logger.LogInformation("Iniciando creación de actividad: {NombreActividad}", dto.Actividad.Nombre);

        await procedureRepository.BeginTransactionAsync();
        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);
            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputNumber("PN_ID_ACTIVIDAD")
                .WithOutputVarchar("PV_MENSAJE", 4000)
                .WithOutputNumber("PN_CODIGO")
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_CREAR_ACTIVIDAD_JSON", dyParam);

            var idActividad = dyParam.Get<int?>("PN_ID_ACTIVIDAD");
            var mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty;
            var codigo = dyParam.Get<int?>("PN_CODIGO") ?? 0;

            if (idActividad.HasValue && idActividad > 0)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Actividad creada exitosamente: {NombreActividad}, ID: {IdActividad}", dto.Actividad.Nombre, idActividad);
                return new Respuesta { Exito = 1, Mensaje = mensaje, IdEntidad = idActividad };
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Creación de actividad fallida: {NombreActividad}, Código: {Codigo}, Mensaje: {Mensaje}", dto.Actividad.Nombre, codigo, mensaje);
                return new Respuesta { Exito = 0, Codigo = codigo, Mensaje = mensaje };
            }
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en creación de actividad: {NombreActividad}", dto.Actividad.Nombre);
            return new Respuesta { Codigo = -1, Mensaje = $"Ocurrió un error al crear la actividad. {ex.Message}" };
        }
    }

    public async Task<Respuesta> CrearHorarioAsync(HorarioCreacionIntegracionDto dto)
    {
        logger.LogInformation("Iniciando creación de horario para la actividad ID: {IdActividad}", dto.IdActividad);

        await procedureRepository.BeginTransactionAsync();
        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);
            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputNumber("PN_ID_HORARIO")
                .WithOutputVarchar("PV_MENSAJE", 4000)
                .WithOutputNumber("PN_CODIGO")
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_CREAR_HORARIO_JSON", dyParam);

            var idHorario = dyParam.Get<int?>("PN_ID_HORARIO");
            var mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty;
            var codigo = dyParam.Get<int?>("PN_CODIGO") ?? 0;

            if (idHorario.HasValue && idHorario > 0)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Horario creado exitosamente. ID: {IdHorario}", idHorario);
                return new Respuesta { Exito = 1, Mensaje = mensaje, IdEntidad = idHorario };
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Creación de horario fallida para actividad ID: {IdActividad}, Código: {Codigo}, Mensaje: {Mensaje}", dto.IdActividad, codigo, mensaje);
                return new Respuesta { Exito = 0, Codigo = codigo, Mensaje = mensaje };
            }
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en creación de horario para actividad ID: {IdActividad}", dto.IdActividad);
            return new Respuesta { Codigo = -1, Mensaje = $"Ocurrió un error al crear el horario. {ex.Message}" };
        }
    }

    public async Task<Respuesta> InscribirParticipanteAsync(InscripcionParticipanteDto dto)
    {
        logger.LogInformation("Iniciando inscripción de participante {IdUsuario} en actividad {IdActividad}", dto.IdUsuarioVoluntario, dto.IdActividad);

        await procedureRepository.BeginTransactionAsync();
        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);
            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputNumber("PN_ID_PARTICIPANTE")
                .WithOutputVarchar("PV_MENSAJE", 4000)
                .WithOutputNumber("PN_CODIGO")
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_INSCRIBIR_PARTICIPANTE_JSON", dyParam);

            var idParticipante = dyParam.Get<int?>("PN_ID_PARTICIPANTE");
            var mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty;
            var codigo = dyParam.Get<int?>("PN_CODIGO") ?? 0;

            if (idParticipante.HasValue && idParticipante > 0)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Inscripción exitosa. ID: {IdParticipante}", idParticipante);
                return new Respuesta { Exito = 1, Mensaje = mensaje, IdEntidad = idParticipante };
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Inscripción fallida para usuario {IdUsuario} en actividad {IdActividad}. Código: {Codigo}, Mensaje: {Mensaje}", dto.IdUsuarioVoluntario, dto.IdActividad, codigo, mensaje);
                return new Respuesta { Exito = 0, Codigo = codigo, Mensaje = mensaje };
            }
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en inscripción de participante para actividad ID: {IdActividad}", dto.IdActividad);
            return new Respuesta { Codigo = -1, Mensaje = $"Ocurrió un error en la inscripción. {ex.Message}" };
        }
    }

    public async Task<Respuesta> AsignarRolAsync(AsignacionRolDto dto)
    {
        logger.LogInformation("Iniciando asignación de rol {IdRol} a usuario {IdUsuarioAsignado} en organización {IdOrganizacion}", dto.IdRol, dto.IdUsuarioAsignado, dto.IdOrganizacion);

        await procedureRepository.BeginTransactionAsync();
        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);
            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithOutputNumber("PN_ID_ASIGNACION")
                .WithOutputVarchar("PV_MENSAJE", 4000)
                .WithOutputNumber("PN_CODIGO")
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_ASIGNAR_ROL_JSON", dyParam);

            var idAsignacion = dyParam.Get<int?>("PN_ID_ASIGNACION");
            var mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty;
            var codigo = dyParam.Get<int?>("PN_CODIGO") ?? 0;

            if (idAsignacion.HasValue && idAsignacion > 0)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Asignación de rol exitosa. ID: {IdAsignacion}", idAsignacion);
                return new Respuesta { Exito = 1, Mensaje = mensaje, IdEntidad = idAsignacion };
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Asignación de rol fallida. Código: {Codigo}, Mensaje: {Mensaje}", codigo, mensaje);
                return new Respuesta { Exito = 0, Codigo = codigo, Mensaje = mensaje };
            }
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en asignación de rol.");
            return new Respuesta { Codigo = -1, Mensaje = $"Ocurrió un error al asignar el rol. {ex.Message}" };
        }
    }

    public async Task<Respuesta> ActualizarPerfilCompletoAsync(ActualizacionPerfilCompletoDto dto)
    {
        logger.LogInformation("Iniciando actualización de perfil para el perfil ID: {IdPerfil}", dto.Perfil.IdPerfil);

        await procedureRepository.BeginTransactionAsync();
        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);
            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithStandardOutputs()
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_ACTUALIZAR_PERFIL_COMPLETO_JSON", dyParam);

            var exito = dyParam.Get<int>("PN_EXITO");
            var mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty;
            var codigo = dyParam.Get<int>("PN_CODIGO");

            if (exito == 1)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Actualización de perfil exitosa para el perfil ID: {IdPerfil}", dto.Perfil.IdPerfil);
                return new Respuesta { Exito = 1, Mensaje = mensaje };
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Actualización de perfil fallida para el perfil ID: {IdPerfil}. Código: {Codigo}, Mensaje: {Mensaje}", dto.Perfil.IdPerfil, codigo, mensaje);
                return new Respuesta { Exito = 0, Codigo = codigo, Mensaje = mensaje };
            }
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en la actualización del perfil ID: {IdPerfil}", dto.Perfil.IdPerfil);
            return new Respuesta { Codigo = -1, Mensaje = $"Ocurrió un error al actualizar el perfil. {ex.Message}" };
        }
    }

    public async Task<Respuesta> ActualizarActividadAsync(ActividadActualizacionIntegracionDto dto)
    {
        logger.LogInformation("Iniciando actualización de actividad ID: {IdActividad}", dto.Actividad.IdActividad);

        await procedureRepository.BeginTransactionAsync();
        try
        {
            var jsonPayload = JsonSerializer.Serialize(dto);
            var dyParam = OracleParameterBuilder.Create()
                .WithJsonInput(jsonPayload)
                .WithStandardOutputs()
                .Build();

            await procedureRepository.ExecuteVoidAsync("PKG_INTEGRACION.P_ACTUALIZAR_ACTIVIDAD_JSON", dyParam);

            var exito = dyParam.Get<int>("PN_EXITO");
            var mensaje = dyParam.Get<string>("PV_MENSAJE") ?? string.Empty;
            var codigo = dyParam.Get<int>("PN_CODIGO");
            
            if (exito == 1)
            {
                await procedureRepository.CommitTransactionAsync();
                logger.LogInformation("Actualización de actividad exitosa para la actividad ID: {IdActividad}", dto.Actividad.IdActividad);
                return new Respuesta { Exito = 1, Mensaje = mensaje };
            }
            else
            {
                await procedureRepository.RollbackTransactionAsync();
                logger.LogWarning("Actualización de actividad fallida para la actividad ID: {IdActividad}. Código: {Codigo}, Mensaje: {Mensaje}", dto.Actividad.IdActividad, codigo, mensaje);
                return new Respuesta { Exito = 0, Codigo = codigo, Mensaje = mensaje };
            }
        }
        catch (Exception ex)
        {
            await procedureRepository.RollbackTransactionAsync();
            logger.LogError(ex, "Error crítico en la actualización de la actividad ID: {IdActividad}", dto.Actividad.IdActividad);
            return new Respuesta { Codigo = -1, Mensaje = $"Ocurrió un error al actualizar la actividad. {ex.Message}" };
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
    public int? IdEntidad { get; set; }
}