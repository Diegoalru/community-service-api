using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using community_service_api.DbContext;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UbicacionesController(NewApplicationDbContext db) : ControllerBase
{
    // DTO plano sin constructor con parámetros opcionales
    public class ItemDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
    }

    [AllowAnonymous]
    [HttpGet("Provincias")]
    public IActionResult GetProvincias([FromQuery] int idPais)
    {
        if (idPais <= 0) return BadRequest(new { mensaje = "idPais debe ser válido." });
        var items = db.Provincia
            .Where(p => p.IdPais == idPais)
            .OrderBy(p => p.Nombre)
            .Select(p => new ItemDto { Id = p.IdProvincia, Nombre = p.Nombre })
            .ToList();
        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet("Cantones")]
    public IActionResult GetCantones([FromQuery] int idPais, [FromQuery] int idProvincia)
    {
        if (idPais <= 0 || idProvincia <= 0) return BadRequest(new { mensaje = "idPais e idProvincia deben ser válidos." });
        var items = db.Canton
            .Where(c => c.IdPais == idPais && c.IdProvincia == idProvincia)
            .OrderBy(c => c.Nombre)
            .Select(c => new ItemDto { Id = c.IdCanton, Nombre = c.Nombre, Codigo = c.Codigo })
            .ToList();
        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet("Distritos")]
    public IActionResult GetDistritos([FromQuery] int idPais, [FromQuery] int idProvincia, [FromQuery] int idCanton)
    {
        if (idPais <= 0 || idProvincia <= 0 || idCanton <= 0)
            return BadRequest(new { mensaje = "idPais, idProvincia e idCanton deben ser válidos." });
        var items = db.Distrito
            .Where(d => d.IdPais == idPais && d.IdProvincia == idProvincia && d.IdCanton == idCanton)
            .OrderBy(d => d.Nombre)
            .Select(d => new ItemDto { Id = d.IdDistrito, Nombre = d.Nombre, Codigo = d.Codigo })
            .ToList();
        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet("CodigoPostal")]
    public IActionResult GetCodigoCompleto([FromQuery] int idDistrito)
    {
        if (idDistrito <= 0) return BadRequest(new { mensaje = "idDistrito debe ser válido." });

        var codigo = (from p in db.Provincia
                join c in db.Canton on p.IdProvincia equals c.IdProvincia
                join d in db.Distrito on c.IdCanton equals d.IdCanton
                where d.IdDistrito == idDistrito
                select (p.Codigo) + (c.Codigo) + (d.Codigo))
            .FirstOrDefault();

        if (codigo == null) return NotFound(new { mensaje = "Distrito no encontrado." });

        return Ok(new { codigo });
    }
}
