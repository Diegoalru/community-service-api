using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoordinadoresActividadController : ControllerBase
{
    private readonly ICoordinadorActividadService _service;

    public CoordinadoresActividadController(ICoordinadorActividadService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var coordinadores = await _service.GetAllAsync();
        return Ok(coordinadores);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var coordinador = await _service.GetByIdAsync(id);
        if (coordinador is null)
        {
            return NotFound();
        }

        return Ok(coordinador);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CoordinadorActividadCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdCoordinadorActividad }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CoordinadorActividadUpdateDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
