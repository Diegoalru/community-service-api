using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificacionesParticipacionController : ControllerBase
{
    private readonly ICertificacionParticipacionService _service;

    public CertificacionesParticipacionController(ICertificacionParticipacionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var certificaciones = await _service.GetAllAsync();
        return Ok(certificaciones);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var certificacion = await _service.GetByIdAsync(id);
        if (certificacion is null)
        {
            return NotFound();
        }

        return Ok(certificacion);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CertificacionParticipacionCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdCertificacion }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CertificacionParticipacionUpdateDto dto)
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
