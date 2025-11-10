using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class CertificacionParticipacionDto
{
    public Guid IdCertificacion { get; set; }

    public DateTime FechaEmision { get; set; }

    public int HorasTotales { get; set; }

    public int DiasTotales { get; set; }

    public DateTime FechaPrimeraAsistencia { get; set; }

    public DateTime FechaUltimaAsistencia { get; set; }

    public char Situacion { get; set; }

    public string? Observaciones { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public int? IntentosEnvio { get; set; }

    public DateTime? UltimoIntentoEnvio { get; set; }

    public string? UltimoErrorEnvio { get; set; }

    public char Estado { get; set; }
}

public class CertificacionParticipacionCreateDto
{
    [Required]
    public int IdParticipanteActividad { get; set; }

    [Required]
    public int IdActividad { get; set; }

    [Required]
    public int IdOrganizacion { get; set; }

    [Required]
    public int IdUsuarioVoluntario { get; set; }

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

    public DateTime? FechaEnvio { get; set; }

    public int? IntentosEnvio { get; set; }

    public DateTime? UltimoIntentoEnvio { get; set; }

    [MaxLength(2000)]
    public string? UltimoErrorEnvio { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; } = 'A';
}

public class CertificacionParticipacionUpdateDto : CertificacionParticipacionCreateDto
{
}
