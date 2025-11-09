using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class CoordinadorActividadDto
{
    public int IdCoordinadorActividad { get; set; }

    public int IdOrganizacion { get; set; }

    public int IdActividad { get; set; }

    public int IdUsuarioCoordinador { get; set; }

    public DateTime FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public char Estado { get; set; }
}

public class CoordinadorActividadCreateDto
{
    [Required]
    public int IdOrganizacion { get; set; }

    [Required]
    public int IdActividad { get; set; }

    [Required]
    public int IdUsuarioCoordinador { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class CoordinadorActividadUpdateDto : CoordinadorActividadCreateDto
{
}
