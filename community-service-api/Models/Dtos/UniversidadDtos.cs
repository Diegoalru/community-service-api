using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class UniversidadDto
{
    public int IdUniversidad { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Siglas { get; set; } = string.Empty;

    public char Estado { get; set; }
}

public class UniversidadCreateDto
{
    [Required]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Siglas { get; set; } = string.Empty;

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class UniversidadUpdateDto : UniversidadCreateDto
{
}

