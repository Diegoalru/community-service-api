using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class HorarioActividad
{
    [Key]
    public Guid IdHorarioActividad { get; set; }

    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdActividad { get; set; }

    [Required]
    public Guid IdUsuario { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public TimeSpan HoraInicio { get; set; }

    [Required]
    public TimeSpan HoraFin { get; set; }

    [MaxLength(255)]
    public string? Descripcion { get; set; }

    [Required]
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow;

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
