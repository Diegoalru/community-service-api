using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class RolUsuarioOrganizacionDto
{
    public Guid IdRolUsuarioOrganizacion { get; set; }
    public Guid IdOrganizacion { get; set; }
    public Guid IdUsuarioAsignado { get; set; }
    public Guid IdUsuarioAdministrador { get; set; }
    public int IdRol { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public char Estado { get; set; }
}

public class RolUsuarioOrganizacionCreateDto
{
    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdUsuarioAsignado { get; set; }

    [Required]
    public Guid IdUsuarioAdministrador { get; set; }

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
