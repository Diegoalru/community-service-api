using community_service_api.Models.Dtos;
using community_service_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegracionController(IIntegracionService integracionService) : ControllerBase
{
    private const string UsuarioRegistradoExitosamenteMensaje =
        "Usuario registrado exitosamente. Por favor verifique su correo.";

    private const string UsuarioInicioSesionExitosamenteMensaje = "Inicio de sesión exitoso.";

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

    [HttpPost("CambiarPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CambiarPassword([FromBody] ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // En una aplicación real, el nombre de usuario se obtendría de los claims del usuario autenticado.
        // Por ejemplo: var username = User.Identity.Name;
        // Y se compararía con dto.Username para asegurar que un usuario solo puede cambiar su propia contraseña.
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
}