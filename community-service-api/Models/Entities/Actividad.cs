using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class Actividad
{
    [Key]
    public Guid IdActividad { get; set; }

    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdUsuarioCreador { get; set; }

    [Required]
    public int IdCategoria { get; set; }

    [Required]
    public int IdPais { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Lugar { get; set; } = string.Empty;

    [Required]
    public DateTime FechaInicio { get; set; }

    [Required]
    public DateTime FechaFin { get; set; }

    [Required]
    public int Horas { get; set; }

    [Required]
    public int Cupos { get; set; }

    [Required]
    [RegularExpression("[PICFA]")]
    public char Situacion { get; set; }

    [Required]
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow;

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
