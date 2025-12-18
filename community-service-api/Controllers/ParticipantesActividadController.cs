using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;
using System.Security.Claims;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantesActividadController : ControllerBase
{
    private readonly IParticipanteActividadService _service;

    public ParticipantesActividadController(IParticipanteActividadService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var participantes = await _service.GetAllAsync();
        return Ok(participantes);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var participante = await _service.GetByIdAsync(id);
        if (participante is null)
        {
            return NotFound();
        }

        return Ok(participante);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ParticipanteActividadCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdParticipanteActividad }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ParticipanteActividadUpdateDto dto)
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

    [HttpPut("{id:int}/situacion")]
    public async Task<IActionResult> CambiarSituacion(int id, [FromBody] SituacionUpdateRequestDto dto)
    {
        var idUsuarioSolicitante = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _service.CambiarSituacionAsync(id, dto, idUsuarioSolicitante);
        return NoContent();
    }
}

