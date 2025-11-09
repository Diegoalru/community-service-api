using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class HorarioActividadDto
{
    public int IdHorarioActividad { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdActividad { get; set; }

    public int IdUsuario { get; set; }

    public DateTime Fecha { get; set; }

    public DateTime HoraInicio { get; set; }

    public DateTime HoraFin { get; set; }

    public string? Descripcion { get; set; }

    public char Situacion { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public char Estado { get; set; }
}

public class HorarioActividadCreateDto
{
    [Required]
    public int IdOrganizacion { get; set; }

    [Required]
    public int IdActividad { get; set; }

    [Required]
    public int IdUsuario { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public DateTime HoraInicio { get; set; }

    [Required]
    public DateTime HoraFin { get; set; }

    [MaxLength(255)]
    public string? Descripcion { get; set; }

    [Required]
    [RegularExpression("[IPCAF]")]
    public char Situacion { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class HorarioActividadUpdateDto : HorarioActividadCreateDto
{
}
