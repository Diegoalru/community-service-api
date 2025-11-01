using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Entities;

public class CertificacionParticipacion
{
    [Key]
    public Guid IdCertificacion { get; set; }

    [Required]
    public Guid IdParticipanteActividad { get; set; }

    [Required]
    public Guid IdActividad { get; set; }

    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdUsuarioVoluntario { get; set; }

    [Required]
    public DateTime FechaEmision { get; set; }

    [Required]
    public int HorasTotales { get; set; }

    [Required]
    public int DiasTotales { get; set; }

    [Required]
    public DateTime FechaPrimeraAsistencia { get; set; }

    [Required]
    public DateTime FechaUltimaAsistencia { get; set; }

    [Required]
    [RegularExpression("[EPA]")]
    public char Situacion { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }

    [Required]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public DateTime? FechaModificacion { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; } = 'A';
}
