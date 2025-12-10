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

    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegistrarCompleto([FromBody] RegistroCompletoDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        RespuestaRegistro respuesta;
        
        try
        {
            respuesta = await integracionService.RegistrarUsuarioCompletoAsync(dto);
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
}