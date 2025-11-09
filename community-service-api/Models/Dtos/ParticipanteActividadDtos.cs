using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class ParticipanteActividadDto
{
    public int IdParticipanteActividad { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdActividad { get; set; }

    public int IdHorarioActividad { get; set; }

    public int IdUsuarioVoluntario { get; set; }

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
    public int IdOrganizacion { get; set; }

    [Required]
    public int IdActividad { get; set; }

    [Required]
    public int IdHorarioActividad { get; set; }

    [Required]
    public int IdUsuarioVoluntario { get; set; }

    [Required]
    public DateTime FechaInscripcion { get; set; }

    public DateTime? FechaRetiro { get; set; }

    [Required]
    [RegularExpression("[IARCF]")]
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
