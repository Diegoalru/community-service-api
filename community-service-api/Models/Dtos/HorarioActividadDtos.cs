using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class HorarioActividadDto
{
    public Guid IdHorarioActividad { get; set; }
    public Guid IdOrganizacion { get; set; }
    public Guid IdActividad { get; set; }
    public Guid IdUsuario { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public char Estado { get; set; }
}

public class HorarioActividadCreateDto
{
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

    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class HorarioActividadUpdateDto : HorarioActividadCreateDto
{
}
