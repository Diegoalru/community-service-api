using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasActividadController : ControllerBase
{
    private readonly ICategoriaActividadService _service;

    public CategoriasActividadController(ICategoriaActividadService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _service.GetAllAsync();
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _service.GetByIdAsync(id);
        if (categoria is null)
        {
            return NotFound();
        }

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoriaActividadCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdCategoriaActividad }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaActividadUpdateDto dto)
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
