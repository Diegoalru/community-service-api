using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class CertificacionParticipacionDto
{
    public Guid IdCertificacion { get; set; }
    public Guid IdParticipanteActividad { get; set; }
    public Guid IdActividad { get; set; }
    public Guid IdOrganizacion { get; set; }
    public Guid IdUsuarioVoluntario { get; set; }
    public DateTime FechaEmision { get; set; }
    public int HorasTotales { get; set; }
    public int DiasTotales { get; set; }
    public DateTime FechaPrimeraAsistencia { get; set; }
    public DateTime FechaUltimaAsistencia { get; set; }
    public char Situacion { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public char Estado { get; set; }
}

public class CertificacionParticipacionCreateDto
{
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

    public DateTime? FechaRegistro { get; set; }
    public DateTime? FechaModificacion { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; } = 'A';
}

public class CertificacionParticipacionUpdateDto : CertificacionParticipacionCreateDto
{
}
