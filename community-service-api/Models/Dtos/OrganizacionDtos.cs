using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class OrganizacionDto
{
    public int IdOrganizacion { get; set; }

    public int IdUsuarioCreador { get; set; }

    public int IdUbicacion { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? SitioWeb { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public char Estado { get; set; }
}

public class OrganizacionCreateDto
{
    [Required]
    public int IdUsuarioCreador { get; set; }

    [Required]
    public int IdUbicacion { get; set; }

    [Required]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefono { get; set; }

    [MaxLength(255)]
    public string? SitioWeb { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class OrganizacionUpdateDto : OrganizacionCreateDto
{
}
