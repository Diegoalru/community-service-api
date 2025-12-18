using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HorariosActividadController : ControllerBase
{
    private readonly IHorarioActividadService _service;

    public HorariosActividadController(IHorarioActividadService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var horarios = await _service.GetAllAsync();
        return Ok(horarios);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var horario = await _service.GetByIdAsync(id);
        if (horario is null)
        {
            return NotFound();
        }

        return Ok(horario);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HorarioActividadCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdHorarioActividad }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] HorarioActividadUpdateDto dto)
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
    
    [HttpGet("{horarioId:int}/participantes")]
    public async Task<IActionResult> GetParticipantesByHorario(int horarioId)
    {
        var participantes = await _service.GetParticipantesByHorarioAsync(horarioId);
        return Ok(participantes);
    }
}
