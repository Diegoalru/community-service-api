using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class CoordinadorActividadDto
{
    public Guid IdCoordinadorActividad { get; set; }
    public Guid IdOrganizacion { get; set; }
    public Guid IdActividad { get; set; }
    public Guid IdUsuarioCoordinador { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public char Estado { get; set; }
}

public class CoordinadorActividadCreateDto
{
    [Required]
    public Guid IdOrganizacion { get; set; }

    [Required]
    public Guid IdActividad { get; set; }

    [Required]
    public Guid IdUsuarioCoordinador { get; set; }

    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class CoordinadorActividadUpdateDto : CoordinadorActividadCreateDto
{
}
