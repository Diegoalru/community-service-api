using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using community_service_api.Models.Dtos;
using community_service_api.Services;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await usuarioService.GetAllAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await usuarioService.GetByIdAsync(id);
        if (usuario is null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioCreateDto dto)
    {
        var created = await usuarioService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.IdUsuario }, created);
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateWithProcedure([FromBody] UsuarioCreateDtoTest dto)
    {
        var newUserId = await usuarioService.CreateUsuarioWithProcedureAsync(dto);
        
        var newUser = await usuarioService.GetByIdAsync(newUserId);

        return CreatedAtAction(nameof(GetById), new { id = newUserId }, newUser);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateDto dto)
    {
        var updated = await usuarioService.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await usuarioService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
