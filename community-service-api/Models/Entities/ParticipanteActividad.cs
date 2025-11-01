using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class ParticipanteActividad
{
    [Key]
    public Guid IdParticipanteActividad { get; set; }

    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdActividad { get; set; }

    [Required]
    public Guid IdHorarioActividad { get; set; }

    [Required]
    public Guid IdUsuarioVoluntario { get; set; }

    [Required]
    public DateTime FechaInscripcion { get; set; }

    public DateTime? FechaRetiro { get; set; }

    [Required]
    [RegularExpression("[ARCFP]")]
    public char Situacion { get; set; }

    [Required]
    public DateTime FechaDesde { get; set; } = DateTime.UtcNow;

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}
