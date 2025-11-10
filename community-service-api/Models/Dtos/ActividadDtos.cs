using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class ActividadDto
{
    public int IdActividad { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? Horas { get; set; }

    public int Cupos { get; set; }

    public char Situacion { get; set; }

    public char Estado { get; set; }
}

public class ActividadCreateDto
{
    [Required]
    public int IdOrganizacion { get; set; }

    [Required]
    public int IdUsuarioCreador { get; set; }

    [Required]
    public int IdCategoria { get; set; }

    [Required]
    public int IdUbicacion { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? Horas { get; set; }

    [Required]
    public int Cupos { get; set; }

    [Required]
    [RegularExpression("[IPCFA]")]
    public char Situacion { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class ActividadUpdateDto : ActividadCreateDto
{
}
