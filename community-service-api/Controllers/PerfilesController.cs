using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerfilesController : ControllerBase
{
    private readonly IPerfilService _service;

    public PerfilesController(IPerfilService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var perfiles = await _service.GetAllAsync();
        return Ok(perfiles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var perfil = await _service.GetByIdAsync(id);
        if (perfil is null)
        {
            return NotFound();
        }

        return Ok(perfil);
    }

    [HttpGet("{id:int}/detalle")]
    public async Task<IActionResult> GetDetalleById(int id)
    {
        var perfil = await _service.GetDetalleByIdAsync(id);
        if (perfil is null)
        {
            return NotFound();
        }

        return Ok(perfil);
    }

    [HttpGet("usuario/{idUsuario:int}/detalle")]
    public async Task<IActionResult> GetDetalleByUsuarioId(int idUsuario)
    {
        var perfil = await _service.GetDetalleByUsuarioIdAsync(idUsuario);
        if (perfil is null)
        {
            return NotFound();
        }

        return Ok(perfil);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PerfilCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdPerfil }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PerfilUpdateDto dto)
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
