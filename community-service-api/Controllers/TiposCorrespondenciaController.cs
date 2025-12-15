using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using community_service_api.DbContext;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TiposCorrespondenciaController(NewApplicationDbContext db) : ControllerBase
{
    public class TipoCorrespondenciaDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetAll()
    {
        var items = db.TipoCorrespondencia
            .OrderBy(t => t.Descripcion)
            .Select(t => new TipoCorrespondenciaDto { Id = t.IdTipoCorrespondencia, Descripcion = t.Descripcion })
            .ToList();
        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var t = db.TipoCorrespondencia.FirstOrDefault(x => x.IdTipoCorrespondencia == id);
        if (t == null) return NotFound();
        return Ok(new TipoCorrespondenciaDto { Id = t.IdTipoCorrespondencia, Descripcion = t.Descripcion });
    }
}
