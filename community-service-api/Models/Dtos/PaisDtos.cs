using System;
using System.ComponentModel.DataAnnotations;

namespace community_service_api.Models.Dtos;

public class PaisDto
{
    public int IdPais { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public char Estado { get; set; }
}

public class PaisCreateDto
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    [Required]
    [RegularExpression("[AI]")]
    public char Estado { get; set; }
}

public class PaisUpdateDto : PaisCreateDto
{
}
