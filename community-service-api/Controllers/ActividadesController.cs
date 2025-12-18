using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;
using Microsoft.AspNetCore.Http;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActividadesController : ControllerBase
{
    private readonly IActividadService _service;

    public ActividadesController(IActividadService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var actividades = await _service.GetAllAsync();
        return Ok(actividades);
    }

    [HttpGet("available-activities")]
    public async Task<IActionResult> GetVigentesDetalle([FromQuery] int idUsuario)
    {
        if (idUsuario <= 0)
        {
            return BadRequest(new { message = "El parámetro 'idUsuario' debe ser mayor a 0." });
        }

        var actividades = await _service.GetVigentesDetalleAsync(idUsuario);
        return Ok(actividades);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var actividad = await _service.GetByIdAsync(id);
        if (actividad is null)
        {
            return NotFound();
        }

        return Ok(actividad);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ActividadCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdActividad }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ActividadUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("inscribir-usuario")]
    public async Task<IActionResult> InscribirUsuario([FromBody] InscribirUsuarioActividadRequestDto dto)
    {
        try
        {
            var result = await _service.InscribirUsuarioAsync(dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("desinscribir-usuario")]
    public async Task<IActionResult> DesinscribirUsuario([FromBody] InscribirUsuarioActividadRequestDto dto)
    {
        try
        {
            var result = await _service.DesinscribirUsuarioAsync(dto);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
