using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesUsuarioOrganizacionController : ControllerBase
{
    private readonly IRolUsuarioOrganizacionService _service;

    public RolesUsuarioOrganizacionController(IRolUsuarioOrganizacionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _service.GetAllAsync();
        return Ok(roles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var rol = await _service.GetByIdAsync(id);
        if (rol is null)
        {
            return NotFound();
        }

        return Ok(rol);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RolUsuarioOrganizacionCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdRolUsuarioOrganizacion }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] RolUsuarioOrganizacionUpdateDto dto)
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
}
