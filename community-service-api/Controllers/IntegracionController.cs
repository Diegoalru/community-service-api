using community_service_api.Models;
using community_service_api.Models.Dtos;
using community_service_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegracionController(IIntegracionService integracionService) : ControllerBase
{
    private const string UsuarioRegistradoExitosamenteMensaje =
        "Usuario registrado exitosamente. Por favor verifique su correo.";

    private const string UsuarioInicioSesionExitosamenteMensaje = "Inicio de sesión exitoso.";

    [AllowAnonymous]
    [HttpPost("RegistroUsuario")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegistroUsuario([FromBody] RegistroCompletoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Respuesta respuesta;

        try
        {
            respuesta = await integracionService.RegistroUsuarioCompletoAsync(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }

        return respuesta.Codigo switch
        {
            0 => Ok(new
            {
                mensaje = UsuarioRegistradoExitosamenteMensaje,
                token = respuesta.Token,
                idUsuario = respuesta.IdUsuario
            }),
            -1 => StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo }),
            _ => BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo })
        };
    }

    [AllowAnonymous]
    [HttpPost("IniciarSesion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> IniciarSesion([FromBody] UsuarioLoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Respuesta respuesta;

        try
        {
            respuesta = await integracionService.InicioSesionAsync(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }

        return respuesta.Exito switch
        {
            1 => Ok(new
            {
                mensaje = UsuarioInicioSesionExitosamenteMensaje,
                idUsuario = respuesta.IdUsuario,
                token = respuesta.Token
            }),
            -1 => StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo }),
            _ => BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo })
        };
    }

    [AllowAnonymous]
    [HttpGet("ActivarCuenta")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActivarCuenta([FromQuery] string token)
    {
        if (string.IsNullOrEmpty(token))
            return BadRequest(new { mensaje = "El token es requerido." });

        Respuesta respuesta;

        try
        {
            respuesta = await integracionService.ActivarCuentaAsync(token);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }

        return respuesta.Exito switch
        {
            1 => Ok(new
            {
                mensaje = "¡Tu cuenta ha sido activada!",
                idUsuario = respuesta.IdUsuario
            }),
            -1 => StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo }),
            _ => BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo })
        };
    }

    [AllowAnonymous]
    [HttpPost("SolicitarRecuperacionPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SolicitarRecuperacionPassword([FromBody] RequestPasswordRecoveryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Respuesta respuesta;

        try
        {
            respuesta = await integracionService.SolicitarRecuperacionPasswordAsync(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }

        if (respuesta.Exito == 1)
            // Se devuelve siempre un mensaje genérico para no revelar si un correo electrónico está registrado en el sistema.
            return Ok(new
            {
                mensaje =
                    "Si existe una cuenta asociada a este correo, recibirás un enlace para restablecer tu contraseña."
            });

        if (respuesta.Codigo == -1)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

        return Ok(new
        {
            mensaje = "Si existe una cuenta asociada a este correo, recibirás un enlace para restablecer tu contraseña."
        });
    }

    [AllowAnonymous]
    [HttpPost("RestablecerPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RestablecerPassword([FromBody] ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Respuesta respuesta;

        try
        {
            respuesta = await integracionService.RestablecerPasswordAsync(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }

        if (respuesta.Exito == 1) return Ok(new { mensaje = "Contraseña restablecida exitosamente." });

        if (respuesta.Codigo == -1)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("CambiarPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CambiarPassword([FromBody] ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validar que el usuario autenticado coincida con el del DTO
        var usernameClaim = User.FindFirst("username")?.Value ?? User.Identity?.Name;
        if (string.IsNullOrEmpty(usernameClaim))
            return Unauthorized(new { mensaje = "No se pudo determinar el usuario autenticado." });

        if (!string.Equals(usernameClaim, dto.Username, StringComparison.OrdinalIgnoreCase))
            return StatusCode(StatusCodes.Status403Forbidden,
                new { mensaje = "No tiene permiso para cambiar la contraseña de otro usuario." });

        Respuesta respuesta;

        try
        {
            respuesta = await integracionService.CambiarPasswordAsync(dto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }

        if (respuesta.Exito == 1) return Ok(new { mensaje = "Contraseña actualizada exitosamente." });

        if (respuesta.Codigo == -1)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [AllowAnonymous]
    [HttpPost("ReenviarActivacion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReenviarActivacion([FromBody] ResendActivationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await integracionService.ReenviarActivacionAsync(dto);
            return Ok(new { mensaje = "Si su cuenta no está activada, se ha enviado un nuevo correo de activación." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("CrearOrganizacion")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrearOrganizacion([FromBody] OrganizacionCreacionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var respuesta = await integracionService.CrearOrganizacionAsync(dto);
            if (respuesta.Exito == 1)
                return CreatedAtAction(nameof(CrearOrganizacion), new { id = respuesta.IdEntidad }, new { id = respuesta.IdEntidad, mensaje = respuesta.Mensaje });
            
            if(respuesta.Codigo == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

            return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("CrearActividad")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrearActividad([FromBody] ActividadCreacionIntegracionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        try
        {
            var respuesta = await integracionService.CrearActividadAsync(dto);
            if (respuesta.Exito == 1)
                return CreatedAtAction(nameof(CrearActividad), new { id = respuesta.IdEntidad }, new { id = respuesta.IdEntidad, mensaje = respuesta.Mensaje });

            if(respuesta.Codigo == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

            return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("CrearHorario")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CrearHorario([FromBody] HorarioCreacionIntegracionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var respuesta = await integracionService.CrearHorarioAsync(dto);
            if (respuesta.Exito == 1)
                return CreatedAtAction(nameof(CrearHorario), new { id = respuesta.IdEntidad }, new { id = respuesta.IdEntidad, mensaje = respuesta.Mensaje });

            if(respuesta.Codigo == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

            return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("InscribirParticipante")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InscribirParticipante([FromBody] InscripcionParticipanteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var respuesta = await integracionService.InscribirParticipanteAsync(dto);
            if (respuesta.Exito == 1)
                return CreatedAtAction(nameof(InscribirParticipante), new { id = respuesta.IdEntidad }, new { id = respuesta.IdEntidad, mensaje = respuesta.Mensaje });

            if(respuesta.Codigo == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

            return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("AsignarRol")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AsignarRol([FromBody] AsignacionRolDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var respuesta = await integracionService.AsignarRolAsync(dto);
            if (respuesta.Exito == 1)
                return CreatedAtAction(nameof(AsignarRol), new { id = respuesta.IdEntidad }, new { id = respuesta.IdEntidad, mensaje = respuesta.Mensaje });

            if(respuesta.Codigo == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

            return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("ActualizarPerfil")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarPerfil([FromBody] ActualizacionPerfilCompletoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var respuesta = await integracionService.ActualizarPerfilCompletoAsync(dto);
            if (respuesta.Exito == 1)
                return Ok(new { mensaje = respuesta.Mensaje });

            if(respuesta.Codigo == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

            return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("ActualizarActividad")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarActividad([FromBody] ActividadActualizacionIntegracionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var respuesta = await integracionService.ActualizarActividadAsync(dto);
            if (respuesta.Exito == 1)
                return Ok(new { mensaje = respuesta.Mensaje });
            
            if(respuesta.Codigo == -1)
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });

            return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("GetOrganizacionesConEstado")]
    public async Task<IActionResult> GetOrganizacionesConEstado([FromBody] GetOrganizacionesConEstadoDto dto)
    {
        var respuesta = await integracionService.GetOrganizacionesConEstadoAsync(dto);
        if (respuesta.Exito == 1)
            return Content(respuesta.JsonResponse ?? "[]", "application/json");
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("GestionarVoluntariado")]
    public async Task<IActionResult> GestionarVoluntariado([FromBody] GestionarVoluntariadoDto dto)
    {
        var respuesta = await integracionService.GestionarVoluntariadoAsync(dto);
        if (respuesta.Exito == 1)
            return Ok(new { mensaje = respuesta.Mensaje });
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("GetUsuariosPorOrg")]
    public async Task<IActionResult> GetUsuariosPorOrg([FromBody] GetUsuariosPorOrgDto dto)
    {
        var respuesta = await integracionService.GetUsuariosPorOrgAsync(dto);
        if (respuesta.Exito == 1)
            return Content(respuesta.JsonResponse ?? "[]", "application/json");
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("EliminarUsuarioOrg")]
    public async Task<IActionResult> EliminarUsuarioOrg([FromBody] EliminarUsuarioOrgDto dto)
    {
        var respuesta = await integracionService.EliminarUsuarioOrgAsync(dto);
        if (respuesta.Exito == 1)
            return Ok(new { mensaje = respuesta.Mensaje });
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }
    
    [Authorize]
    [HttpPut("ActualizarUsuarioOrg")]
    public async Task<IActionResult> ActualizarUsuarioOrg([FromBody] ActualizarUsuarioOrgDto dto)
    {
        var respuesta = await integracionService.ActualizarUsuarioOrgAsync(dto);
        if (respuesta.Exito == 1)
            return Ok(new { mensaje = respuesta.Mensaje });
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }
    
    [Authorize]
    [HttpPost("GetActividadesPorOrg")]
    public async Task<IActionResult> GetActividadesPorOrg([FromBody] GetActividadesPorOrgDto dto)
    {
        var respuesta = await integracionService.GetActividadesPorOrgAsync(dto);
        if (respuesta.Exito == 1)
            return Content(respuesta.JsonResponse ?? "[]", "application/json");
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("GetHorariosPorAct")]
    public async Task<IActionResult> GetHorariosPorAct([FromBody] GetHorariosPorActDto dto)
    {
        var respuesta = await integracionService.GetHorariosPorActAsync(dto);
        if (respuesta.Exito == 1)
            return Content(respuesta.JsonResponse ?? "[]", "application/json");
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("EliminarHorario")]
    public async Task<IActionResult> EliminarHorario([FromBody] EliminarHorarioDto dto)
    {
        var respuesta = await integracionService.EliminarHorarioAsync(dto);
        if (respuesta.Exito == 1)
            return Ok(new { mensaje = respuesta.Mensaje });
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("EliminarActividad")]
    public async Task<IActionResult> EliminarActividad([FromBody] EliminarActividadDto dto)
    {
        var respuesta = await integracionService.EliminarActividadAsync(dto);
        if (respuesta.Exito == 1)
            return Ok(new { mensaje = respuesta.Mensaje });
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }
    
    [Authorize]
    [HttpPut("ActualizarHorario")]
    public async Task<IActionResult> ActualizarHorario([FromBody] ActualizarHorarioDto dto)
    {
        var respuesta = await integracionService.ActualizarHorarioAsync(dto);
        if (respuesta.Exito == 1)
            return Ok(new { mensaje = respuesta.Mensaje });
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("GetOrganizacionById")]
    public async Task<IActionResult> GetOrganizacionById([FromBody] GetByIdDto dto)
    {
        var respuesta = await integracionService.GetOrganizacionByIdAsync(dto);
        if (respuesta.Exito == 1)
            return Content(respuesta.JsonResponse ?? "{}", "application/json");
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("GetActividadById")]
    public async Task<IActionResult> GetActividadById([FromBody] GetByIdDto dto)
    {
        var respuesta = await integracionService.GetActividadByIdAsync(dto);
        if (respuesta.Exito == 1)
            return Content(respuesta.JsonResponse ?? "{}", "application/json");
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPost("GetHorarioById")]
    public async Task<IActionResult> GetHorarioById([FromBody] GetByIdDto dto)
    {
        var respuesta = await integracionService.GetHorarioByIdAsync(dto);
        if (respuesta.Exito == 1)
            return Content(respuesta.JsonResponse ?? "{}", "application/json");
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }

    [Authorize]
    [HttpPut("ActualizarOrganizacion")]
    public async Task<IActionResult> ActualizarOrganizacion([FromBody] ActualizarOrganizacionDto dto)
    {
        var respuesta = await integracionService.ActualizarOrganizacionAsync(dto);
        if (respuesta.Exito == 1)
            return Ok(new { mensaje = respuesta.Mensaje });
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }
    
    [Authorize]
    [HttpPut("CambiarRolUsuario")]
    public async Task<IActionResult> CambiarRolUsuario([FromBody] CambiarRolUsuarioDto dto)
    {
        var respuesta = await integracionService.CambiarRolUsuarioAsync(dto);
        if (respuesta.Exito == 1)
            return Ok(new { mensaje = respuesta.Mensaje });
        return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
    }
}