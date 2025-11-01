using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class RolUsuarioOrganizacion
{
    [Key]
    public Guid IdRolUsuarioOrganizacion { get; set; }

    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdUsuarioAsignado { get; set; }

    [Required]
    public Guid IdUsuarioAdministrador { get; set; }

    [Required]
    public int IdRol { get; set; }

    [Required]
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow;

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
