using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegracionController(IIntegracionService integracionService) : ControllerBase
{
    private const string UsuarioRegistradoExitosamenteMensaje = "Usuario registrado exitosamente. Por favor verifique su correo.";
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
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }

        return respuesta.Codigo switch
        {
            0 => Ok(new
            {
                mensaje = UsuarioRegistradoExitosamenteMensaje,
                token = respuesta.Token,
                idUsuario = respuesta.IdUsuario
            }),
            -1 => StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo }),
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
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno al procesar la solicitud.", detalle = ex.Message });
        }

        return respuesta.Exito switch
        {
            1 => Ok(new
            {
                mensaje = UsuarioInicioSesionExitosamenteMensaje,
                idUsuario = respuesta.IdUsuario
            }),
            -1 => StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo }),
            _ => BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo })
        };
    }
    
}