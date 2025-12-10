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
    [HttpPost("register")]
    public async Task<IActionResult> RegistrarCompleto([FromBody] RegistroCompletoDto dto)
    {
        var respuesta = await integracionService.RegistrarUsuarioCompletoAsync(dto);

        if (respuesta.Codigo == 0)
        {
            // TODO: Aquí integrarías tu servicio de Email (SendGrid/SMTP)
            // EnviarEmail(destinatario: dto.Correspondencia..., token: respuesta.Token);
            
            return Ok(new { 
                mensaje = "Usuario registrado exitosamente. Por favor verifique su correo.", 
                idUsuario = respuesta.IdUsuario,
                // token = respuesta.Token // Opcional: devolverlo solo en desarrollo
            });
        }
        else
        {
            return BadRequest(new { mensaje = respuesta.Mensaje, codigoError = respuesta.Codigo });
        }
    }
}