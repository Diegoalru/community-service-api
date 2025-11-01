using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class CoordinadorActividad
{
    [Key]
    public Guid IdCoordinadorActividad { get; set; }

    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdActividad { get; set; }

    [Required]
    public Guid IdUsuarioCoordinador { get; set; }

    [Required]
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow;

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
