using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;
using Microsoft.AspNetCore.Authorization;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TipoIdentificadorController(ITipoIdentificadorService service) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tipos = await service.GetAllAsync();
        return Ok(tipos);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tipo = await service.GetByIdAsync(id);
        if (tipo is null)
        {
            return NotFound();
        }

        return Ok(tipo);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TipoIdentificadorCreateDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdIdentificador }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TipoIdentificadorUpdateDto dto)
    {
        var updated = await service.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
