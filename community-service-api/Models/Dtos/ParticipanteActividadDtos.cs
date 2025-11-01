using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class ParticipanteActividadDto
{
    public Guid IdParticipanteActividad { get; set; }
    public Guid IdOrganizacion { get; set; }
    public Guid IdActividad { get; set; }
    public Guid IdHorarioActividad { get; set; }
    public Guid IdUsuarioVoluntario { get; set; }
    public DateTime FechaInscripcion { get; set; }
    public DateTime? FechaRetiro { get; set; }
    public char Situacion { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public char Estado { get; set; }
}

public class ParticipanteActividadCreateDto
{
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

    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class ParticipanteActividadUpdateDto : ParticipanteActividadCreateDto
{
}
