using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsistenciasActividadController(IAsistenciaActividadService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] AsistenciaActividadCreateDto dto)
        {
            var res = await service.RegistrarAsync(dto);
            return Ok(res);
        }

        [HttpGet("actividad/{actividadId:int}")]
        public async Task<IActionResult> GetByActividad(int actividadId)
        {
            var res = await service.GetByActividadAsync(actividadId);
            return Ok(res);
        }

        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            var res = await service.GetByUsuarioAsync(usuarioId);
            return Ok(res);
        }

        [HttpGet("usuario/{usuarioId:int}/total")]
        public async Task<IActionResult> TotalHoras(int usuarioId, [FromQuery] int? actividadId)
        {
            var total = await service.GetTotalHorasAsync(usuarioId, actividadId);
            return Ok(new { UsuarioId = usuarioId, ActividadId = actividadId, TotalHoras = total });
        }
    }
}
