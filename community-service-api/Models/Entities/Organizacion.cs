using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class Organizacion
{
    [Key]
    public Guid IdOrganizacion { get; set; }

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

    [Required]
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow;

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
