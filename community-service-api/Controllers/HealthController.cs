using community_service_api.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace community_service_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController(NewApplicationDbContext context) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("CheckDatabaseConnection")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            await context.Database.CanConnectAsync();
            return Ok(new { status = "Connected", message = "Conexión exitosa a Oracle Database" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = "Error", message = ex.Message });
        }
    }
}