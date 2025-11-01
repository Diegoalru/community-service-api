using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class OrganizacionDto
{
    public Guid IdOrganizacion { get; set; }
    public Guid IdUsuarioCreador { get; set; }
    public int IdPais { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string SitioWeb { get; set; } = string.Empty;
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public char Estado { get; set; }
}

public class OrganizacionCreateDto
{
    [Required]
    public Guid IdUsuarioCreador { get; set; }

    [Required]
    public int IdPais { get; set; }

    [Required]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Ciudad { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string SitioWeb { get; set; } = string.Empty;

    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class OrganizacionUpdateDto : OrganizacionCreateDto
{
}
