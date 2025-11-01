using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class ActividadDto
{
    public Guid IdActividad { get; set; }
    public Guid IdOrganizacion { get; set; }
    public Guid IdUsuarioCreador { get; set; }
    public int IdCategoria { get; set; }
    public int IdPais { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Lugar { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int Horas { get; set; }
    public int Cupos { get; set; }
    public char Situacion { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public char Estado { get; set; }
}

public class ActividadCreateDto
{
    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdUsuarioCreador { get; set; }

    [Required]
    public int IdCategoria { get; set; }

    [Required]
    public int IdPais { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Lugar { get; set; } = string.Empty;

    [Required]
    public DateTime FechaInicio { get; set; }

    [Required]
    public DateTime FechaFin { get; set; }

    [Required]
    public int Horas { get; set; }

    [Required]
    public int Cupos { get; set; }

    [Required]
    [RegularExpression("[PICFA]")]
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
