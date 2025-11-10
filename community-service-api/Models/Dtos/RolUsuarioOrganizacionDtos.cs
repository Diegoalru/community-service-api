using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class RolUsuarioOrganizacionDto
{
    public int IdRolUsuarioOrganizacion { get; set; }

    public char Estado { get; set; }
}

public class RolUsuarioOrganizacionCreateDto
{
    [Required]
    public int IdOrganizacion { get; set; }

    [Required]
    public int IdUsuarioAsignado { get; set; }

    [Required]
    public int IdUsuarioAdministrador { get; set; }

    [Required]
    public int IdRol { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class RolUsuarioOrganizacionUpdateDto : RolUsuarioOrganizacionCreateDto
{
}
